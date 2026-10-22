"""Verify migration coverage, references, types and source equivalence.

Run from the repository root: python doc/check_style_resources.py
Requires lxml, also used for an independent XML walk rather than text matching.
"""
from pathlib import Path
import re
import subprocess
from collections import Counter
from lxml import etree as E
from migrate_style_resources import ROOT, FILES, LEDGER, scan

NS = '{http://schemas.microsoft.com/winfx/2006/xaml}'
PARSER = E.XMLParser(remove_blank_text=True, remove_comments=True)
resources = {}
errors = []
# Explicitly reviewed non-size literals. An independent XML walk rejects any
# new numeric property outside these categories, even if the migrator misses it.
NON_SIZE = set('Grid.Column Grid.Row Opacity Value Grid.ColumnSpan Offset RenderTransformOrigin To Grid.RowSpan KeyTime From Angle Panel.ZIndex KeyboardNavigation.TabIndex Minimum StartPoint EndPoint KeySpline Rect Rows Text Viewport ScaleX Duration X Columns ScaleY Maximum Layout GeneratedDuration BeginTime Direction Points hc:PasswordBoxAttach.PasswordLength EndAngle Position LookDirection DotDelayTime Content ConverterParameter EditorButtonText SmallChange Y X2 OffsetAngle GroupName ZIndex Viewbox DesiredFrameRate StrokeDashOffset LargeChange MaxLength UpDirection FieldOfView Y2 PageNumber DotSpeed StartAngle'.split())
for path in (ROOT / 'Basic').rglob('*.xaml'):
    tree = E.parse(str(path))
    keys = [e.get(NS + 'Key') for e in tree.getroot() if e.get(NS + 'Key')]
    for key, count in Counter(keys).items():
        if count > 1:
            errors.append(f'{path}: duplicate {key}')
    for e in tree.getroot():
        key = e.get(NS + 'Key') or ''
        if re.search(r'Token\d+(?:Color)?$', key):
            resources.setdefault(key, e)

ledger = LEDGER.read_text(encoding='utf-8')
for path in FILES:
    text = path.read_text(encoding='utf-8-sig')
    remaining = scan(text)
    if remaining:
        errors.append(f'{path}: {len(remaining)} remaining candidates')
    if f'- [x] `{path.relative_to(ROOT).as_posix()}`' not in ledger:
        errors.append(f'{path}: missing completed file entry')
    for key in re.findall(r'\{(?:Static|Dynamic)Resource (\w+Token\d+)\}', text):
        if key not in resources:
            errors.append(f'{path}: undefined {key}')

    def literal(match):
        key = match[1]
        e = resources[key]
        if E.QName(e).localname == 'SolidColorBrush':
            color_key = re.fullmatch(r'\{DynamicResource (\w+)\}', e.get('Color'))[1]
            return resources[color_key].text.strip()
        return e.text.strip()

    expanded = re.sub(r'\{(?:Static|Dynamic)Resource (\w+Token\d+)\}', literal, text)
    original = subprocess.check_output(['git', 'show', 'HEAD:' + path.as_posix()]).decode('utf-8-sig')
    before = E.tostring(E.fromstring(original.encode(), PARSER), method='c14n')
    after = E.tostring(E.fromstring(expanded.encode(), PARSER), method='c14n')
    if before != after:
        errors.append(f'{path}: source differs after resource expansion')

    # Check GridLength vs Double at the XML use site, including min/max sizes.
    for e in E.fromstring(text.encode()).iter():
        if not isinstance(e.tag, str):
            continue
        tag = E.QName(e).localname
        for attr, value in e.attrib.items():
            prop = e.get('Property', attr) if attr == 'Value' else attr
            if re.fullmatch(r'[-+0-9., ]+', value) and prop not in NON_SIZE:
                errors.append(f'{path}: unreviewed numeric literal {prop}={value}')
            if re.fullmatch(r'#[0-9a-fA-F]{3,8}', value):
                errors.append(f'{path}: literal color {prop}={value}')
            match = re.fullmatch(r'\{StaticResource (\w+Token\d+)\}', value)
            if match and match[1] in resources:
                actual = E.QName(resources[match[1]]).localname
                target_tag, target_prop = tag, attr
                if tag == 'Setter' and attr == 'Value' and e.get('TargetName'):
                    # Resolve names within this template, never across templates.
                    template = next((a for a in e.iterancestors()
                                     if E.QName(a).localname in {'ControlTemplate', 'DataTemplate', 'ItemsPanelTemplate'}), None)
                    if template is not None:
                        targets = template.xpath('.//*[@x:Name=$name or @Name=$name]',
                                                 namespaces={'x': NS[1:-1]}, name=e.get('TargetName'))
                        targets = [t for t in targets if next((a for a in t.iterancestors()
                                   if E.QName(a).localname in {'ControlTemplate', 'DataTemplate', 'ItemsPanelTemplate'}), None) is template]
                        if len(targets) == 1:
                            target_tag = E.QName(targets[0]).localname
                            target_prop = e.get('Property', '').split('.')[-1]
                if target_tag in {'RowDefinition', 'ColumnDefinition'} and target_prop in {'Width', 'Height', 'MinWidth', 'MaxWidth', 'MinHeight', 'MaxHeight'}:
                    expected = 'GridLength' if (target_tag, target_prop) in {('RowDefinition', 'Height'), ('ColumnDefinition', 'Width')} else 'Double'
                    if expected != actual:
                        errors.append(f'{path}:{e.sourceline}: {target_tag}.{target_prop}: {actual}, expected {expected}')

color_keys = {k for k, e in resources.items() if E.QName(e).localname == 'Color'}
for palette in (ROOT / 'Basic/Colors').glob('Colors*.xaml'):
    keys = {e.get(NS + 'Key') for e in E.parse(str(palette)).getroot()}
    if color_keys - keys:
        errors.append(f'{palette}: missing migration colors')
for manifest in ['Theme_GE45.txt', 'Theme_40.txt']:
    lines = (ROOT / manifest).read_text(encoding='utf-8-sig').splitlines()
    if lines.index('Basic\\Colors\\StyleColors.xaml') > lines.index('Basic\\Brushes.xaml'):
        errors.append(f'{manifest}: incorrect color/brush order')

print(f'Files checked: {len(FILES)}; new resource keys: {len(resources)}; errors: {len(errors)}')
for error in errors:
    print(error)
raise SystemExit(bool(errors))
