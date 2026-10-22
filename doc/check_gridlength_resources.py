"""Audit all theme dictionaries, including named template trigger targets.

Run from the repository root: python doc/check_gridlength_resources.py
Bindings are printed for separate source/converter review, not marked as proven.
"""
from collections import defaultdict
from pathlib import Path
import re
from lxml import etree as E

ROOT = Path('Shared/HandyControl_Shared/Themes')
X = '{http://schemas.microsoft.com/winfx/2006/xaml}'
TEMPLATES = {'ControlTemplate', 'DataTemplate', 'ItemsPanelTemplate'}
SIZES = {'Width', 'Height', 'MinWidth', 'MaxWidth', 'MinHeight', 'MaxHeight'}


def tag(element):
    return E.QName(element).localname


def template(element):
    return next((a for a in element.iterancestors() if tag(a) in TEMPLATES), None)


trees = [(p, E.parse(str(p), E.XMLParser(remove_comments=True)))
         for p in sorted(ROOT.rglob('*.xaml'))]
resources = defaultdict(set)
for _, tree in trees:
    for e in tree.iter():
        if e.get(X + 'Key'):
            resources[e.get(X + 'Key')].add(tag(e))

errors, reviews = [], []
definitions = setters = references = 0
for path, tree in trees:
    for e in tree.iter():
        target, values = e, e.attrib.items()
        if tag(e) == 'Setter':
            prop = e.get('Property', '').split('.')[-1]
            owner = template(e)
            if e.get('TargetName') and owner is not None:
                targets = [t for t in owner.iter() if
                           (t.get(X + 'Name') or t.get('Name')) == e.get('TargetName')
                           and template(t) is owner]
                if len(targets) != 1:
                    continue
                target = targets[0]
            else:
                # Include styles directly targeting definition types.
                owner = next((a for a in e.iterancestors() if tag(a) == 'Style'), None)
                if owner is None:
                    continue
                target_type = owner.get('TargetType', '').replace('{x:Type ', '').rstrip('}').split(':')[-1]
                if target_type not in {'RowDefinition', 'ColumnDefinition'}:
                    continue
                target = E.Element(target_type)
            values = [(prop, e.get('Value', ''))]
            if tag(target) in {'RowDefinition', 'ColumnDefinition'}:
                setters += 1
        elif tag(e) in {'RowDefinition', 'ColumnDefinition'}:
            definitions += 1
        else:
            continue
        if tag(target) not in {'RowDefinition', 'ColumnDefinition'}:
            continue
        for prop, value in values:
            prop = prop.split('.')[-1]
            if prop not in SIZES:
                continue
            expected = 'GridLength' if (tag(target), prop) in {
                ('RowDefinition', 'Height'), ('ColumnDefinition', 'Width')} else 'Double'
            location = f'{path}:{e.sourceline}: {tag(target)}.{prop}'
            match = re.fullmatch(r'\{(?:Static|Dynamic)Resource\s+([^{}]+)\}', value)
            if match:
                references += 1
                actual = resources.get(match[1].strip(), set())
                if actual != {expected}:
                    errors.append(f'{location}: {value}: {sorted(actual)}, expected {expected}')
            elif value.startswith('{') or not value:
                reviews.append(f'{location}: {value or "[Setter.Value: review required]"}')

print(f'Files: {len(trees)}; definitions: {definitions}; definition setters: {setters}; resource references: {references}; errors: {len(errors)}; binding/review sites: {len(reviews)}')
for entry in errors:
    print('ERROR', entry)
for entry in reviews:
    if 'Theme.xaml:' not in entry:
        print('REVIEW', entry)
raise SystemExit(bool(errors))
