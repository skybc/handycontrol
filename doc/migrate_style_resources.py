"""One-shot, source-preserving migration; run from the repository root.

The ledger is flushed after each file. Existing bindings and trigger conditions
are deliberately left alone. Generated Theme.xaml files are never edited here.
"""
from pathlib import Path
import re
from collections import defaultdict
from xml.sax.saxutils import escape

ROOT = Path('Shared/HandyControl_Shared/Themes')
FILES = sorted((ROOT / 'Styles').rglob('*.xaml'))
LEDGER = Path('doc/样式资源化检查记录.md')
TAG = re.compile(r'<!--[\s\S]*?-->|<\?[\s\S]*?\?>|</?[^>]+>')
ATTR = re.compile(r'([\w:.-]+)\s*=\s*([\"\'])(.*?)\2', re.S)
NUMBER = re.compile(r'^[+-]?(?:\d+(?:\.\d*)?|\.\d+)(?:[eE][+-]?\d+)?$')
COLORS = set('Transparent Black White Red Green Blue Yellow Gray Grey LightGray DarkGray Silver Orange Purple Pink Brown Cyan Magenta Lime Navy Teal Olive Maroon Aqua Fuchsia Gold WhiteSmoke Gainsboro DimGray DarkRed LightBlue AliceBlue DodgerBlue'.lower().split())
BRUSH = {'Background', 'Foreground', 'BorderBrush', 'Fill', 'Stroke', 'Brush', 'CaretBrush', 'SelectionBrush', 'OpacityMask'}
DOUBLE = {'Width', 'Height', 'MinWidth', 'MinHeight', 'MaxWidth', 'MaxHeight', 'FontSize', 'StrokeThickness', 'RadiusX', 'RadiusY', 'ItemWidth', 'ItemHeight', 'RowHeight', 'MinRowHeight', 'ColumnWidth', 'MinColumnWidth', 'MaxColumnWidth', 'IconSize', 'Size', 'Spacing', 'HorizontalSpacing', 'VerticalSpacing', 'Indent', 'ItemSize', 'SplitterWidth', 'SplitterHeight'}

def kind(prop, value, tag=''):
    prop = prop.split('.')[-1]
    if value.startswith('{'):
        return None
    if prop in BRUSH or prop.endswith(('Brush', 'Color', 'Background', 'Foreground', 'Fill', 'Stroke')):
        if re.fullmatch(r'#[0-9a-fA-F]{3,8}', value) or value.lower() in COLORS:
            return 'Color' if prop.endswith('Color') else 'SolidColorBrush'
    vals = re.split(r'[,\s]+', value.strip())
    if not all(NUMBER.fullmatch(v) for v in vals):
        return None
    if prop in {'Margin', 'Padding', 'BorderThickness', 'MarginOnTheLeft', 'MarginOnTheTop'} or prop.endswith(('Margin', 'Padding')):
        return 'Thickness'
    if prop in {'StrokeDashArray', 'BorderDashArray'}:
        return 'DoubleCollection'
    if prop.endswith('CornerRadius'):
        return 'CornerRadius'
    if prop in DOUBLE or prop.endswith(('FontSize', 'IconSize', 'Width', 'Height', 'Diameter', 'Spacing', 'Thickness')) or prop in {'DotInterval', 'DotOffSet', 'Diameter', 'PanelAreaLength', 'ShadowDepth', 'BlurRadius', 'PopupVerticalOffset', 'PopupHorizontalOffset'} or prop in {'Left', 'Right', 'Top', 'Bottom'}:
        if (tag, prop) in {('RowDefinition', 'Height'), ('ColumnDefinition', 'Width')} or prop == 'TitleWidth':
            return 'GridLength'
        if prop == 'Size' and len(vals) == 2:
            return 'Size'
        if len(vals) == 1:
            return 'sys:Double'
    return None

def scan(text):
    stack = []
    found = []
    for match in TAG.finditer(text):
        raw = match.group()
        if raw.startswith(('<!--', '<?', '<!')):
            continue
        if raw.startswith('</'):
            if stack:
                stack.pop()
            continue
        name = re.match(r'<([^\s/>]+)', raw)[1]
        local = name.split(':')[-1]
        attrs = {m[1]: m[3] for m in ATTR.finditer(raw)}
        style = next((a.get('x:Key', a.get('TargetType', n)) for n, a in reversed(stack) if n == 'Style'), '')
        for a in ATTR.finditer(raw):
            prop = a[1]
            value = a[3]
            if prop in {'From', 'To', 'By'} and local.endswith('Animation'):
                target = attrs.get('Storyboard.TargetProperty', '')
                prop = target.strip('()').split('.')[-1]
                if local == 'ColorAnimation':
                    prop = 'Color'
            if prop == 'Value':
                if local == 'Setter':
                    prop = attrs.get('Property', '')
                elif local.endswith('KeyFrame'):
                    animation = next((a for n, a in reversed(stack) if 'Animation' in n), {})
                    target = animation.get('Storyboard.TargetProperty', '')
                    prop = target.strip('()').split('.')[-1]
                    if not kind(prop, value):
                        continue
                else:
                    continue
            typ = kind(prop, value, local)
            if typ:
                found.append((match.start() + a.start(3), match.start() + a.end(3), prop, value, typ, style, local))
        if not raw.endswith('/>'):
            stack.append((local, attrs))
    return found

def append(path, lines):
    if not lines:
        return
    text = path.read_text(encoding='utf-8-sig')
    text = text.replace('</ResourceDictionary>', '\n    <!-- Styles resource migration: preserve original appearance. -->\n' + '\n'.join(lines) + '\n\n</ResourceDictionary>')
    path.write_text(text, encoding='utf-8', newline='\r\n')

def main():
    import sys
    continuing = '--continue' in sys.argv
    if LEDGER.exists() and not continuing:
        raise SystemExit('Ledger already exists; do not run the migration twice.')
    if not continuing:
        LEDGER.write_text('# 样式资源化检查记录\n\n范围：Styles 全部文件。视觉尺寸包含零值；保留网格比例、几何坐标、非尺寸动画参数、触发条件和业务数据。\n\n状态：修改后仅表示已完成源文件迁移；构建和运行验收单独记录。\n\n## 文件清单\n\n' + '\n'.join(f'- [ ] `{p.relative_to(ROOT).as_posix()}`：待检查' for p in FILES) + '\n\n## 逐项记录\n', encoding='utf-8')
    color_file = ROOT / 'Basic/Colors/StyleColors.xaml'
    if not continuing:
        color_file.write_text('<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">\n</ResourceDictionary>\n', encoding='utf-8')
    for manifest in ([] if continuing else ['Theme_GE45.txt', 'Theme_40.txt']):
        path = ROOT / manifest
        text = path.read_text(encoding='utf-8-sig').replace('Basic\\Brushes.xaml', 'Basic\\Colors\\StyleColors.xaml\nBasic\\Brushes.xaml')
        path.write_text(text, encoding='utf-8', newline='\r\n')
    # Defaults precede brushes in both combine manifests. Palette entries allow
    # dynamic brush colors to follow runtime skin replacement.
    count = 0
    for path in FILES:
        text = path.read_text(encoding='utf-8-sig')
        hits = scan(text)
        entries = defaultdict(list)
        keys = {}
        replacements = []
        details = []
        prefix = path.stem.replace('BaseStyle', '').replace('Style', '') + ('Base' if path.parent.name == 'Base' else 'Style')
        counters = defaultdict(int)
        if continuing:
            for resource in (ROOT / 'Basic').rglob('*.xaml'):
                for name, number in re.findall(re.escape(prefix) + r'(\w+?)Token(\d+)', resource.read_text(encoding='utf-8-sig')):
                    counters[name] = max(counters[name], int(number))
        for start, end, prop, value, typ, style, tag in hits:
            short = prop.split('.')[-1]
            identity = (short, value, typ)
            if identity not in keys:
                counters[short] += 1
                key = f'{prefix}{short}Token{counters[short]}'
                keys[identity] = key
                if typ in {'Color', 'SolidColorBrush'}:
                    color_key = key if typ == 'Color' else key + 'Color'
                    entries['colors'].append(f'    <Color x:Key="{color_key}">{value}</Color>')
                    if typ == 'SolidColorBrush':
                        entries['brushes'].append(f'    <SolidColorBrush x:Key="{key}" Color="{{DynamicResource {color_key}}}" />')
                else:
                    target = 'fonts' if short == 'FontSize' or short.endswith('FontSize') else 'sizes'
                    xml_type = 'system:Double' if target == 'fonts' and typ == 'sys:Double' else typ
                    entries[target].append(f'    <{xml_type} x:Key="{key}">{value}</{xml_type}>')
            key = keys[identity]
            # Static references are valid in frozen drawings and storyboards;
            # DP colors/brushes use DynamicResource to preserve skin overrides.
            mode = 'StaticResource'
            if typ in {'Color', 'SolidColorBrush'} and tag not in {'GeometryDrawing', 'GradientStop', 'Pen'} and not tag.endswith(('KeyFrame', 'Animation')):
                mode = 'DynamicResource'
            replacements.append((start, end, '{' + mode + ' ' + key + '}'))
            line = text.count('\n', 0, start) + 1
            details.append(f'| {line} | `{style or tag}` | `{prop}` | `{value}` | `{key}` |')
        append(color_file, entries['colors'])
        for palette in (ROOT / 'Basic/Colors').glob('Colors*.xaml'):
            append(palette, entries['colors'])
        append(ROOT / 'Basic/Brushes.xaml', entries['brushes'])
        append(ROOT / 'Basic/Sizes.xaml', entries['sizes'])
        append(ROOT / 'Basic/Fonts.xaml', entries['fonts'])
        for start, end, value in reversed(replacements):
            text = text[:start] + value + text[end:]
        if hits:
            path.write_text(text, encoding='utf-8', newline='\r\n')
        ledger = LEDGER.read_text(encoding='utf-8')
        rel = path.relative_to(ROOT).as_posix()
        ledger = ledger.replace(f'- [ ] `{rel}`：待检查', f'- [x] `{rel}`：' + (f'已修改待验证（{len(hits)} 项）' if hits else '静态检查无需修改'))
        if not continuing or hits:
            ledger += f'\n### {rel}' + ('（补充复查）' if continuing else '') + '\n\n'
            ledger += '| 原始行 | 样式/元素 | 属性 | 原值 | 资源键 |\n| --- | --- | --- | --- | --- |\n' + '\n'.join(details) + '\n' if hits else '未发现本次范围内的硬编码颜色或尺寸。\n'
        LEDGER.write_text(ledger, encoding='utf-8')
        count += len(hits)
    print(f'Migrated {count} occurrences in {len(FILES)} files; ledger flushed after every file.')

if __name__ == '__main__':
    main()
