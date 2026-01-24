using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace HandyControl.Collections;

/// <summary>
/// 虚拟化集合，用于支持大数据量显示
/// 只缓存当前可视区的行数据，其他行数据按需从 DataSource 加载
/// </summary>
public class VirtualizingCollection<T> : ObservableCollection<T> where T : class
{
    private IEnumerable _sourceData;
    private List<T> _cachedItems;
    private int _startIndex;
    private int _cacheSize;
    private const int DefaultCacheSize = 50;

    /// <summary>
    /// 数据源 - 支持 IEnumerable 或 IList
    /// </summary>
    public IEnumerable SourceData
    {
        get => _sourceData;
        set
        {
            if (_sourceData != value)
            {
                _sourceData = value;
                ResetAndReload();
            }
        }
    }

    /// <summary>
    /// 缓存大小 - 实际显示行数
    /// </summary>
    public int CacheSize
    {
        get => _cacheSize;
        set
        {
            if (_cacheSize != value)
            {
                _cacheSize = Math.Max(value, 1);
                ResetAndReload();
            }
        }
    }

    /// <summary>
    /// 数据源中的总项目数
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    /// 当前缓存的起始索引
    /// </summary>
    public int StartIndex => _startIndex;

    public VirtualizingCollection() : this(DefaultCacheSize)
    {
    }

    public VirtualizingCollection(int cacheSize)
    {
        _cacheSize = Math.Max(cacheSize, 1);
        _cachedItems = new List<T>(_cacheSize);
        _startIndex = 0;
        TotalCount = 0;
    }

    /// <summary>
    /// 重新加载数据 - 根据起始索引和缓存大小从数据源加载行数据
    /// 同时更新每个数据项的 Index 属性（如果有的话），以反映虚拟滚动的实际行号
    /// </summary>
    public void Reload(int startIndex = 0)
    {
        if (_sourceData == null)
        {
            Clear();
            TotalCount = 0;
            return;
        }

        // 确保起始索引有效
        startIndex = Math.Max(0, startIndex);
        startIndex = Math.Min(startIndex, Math.Max(0, TotalCount - _cacheSize));

        _startIndex = startIndex;

        // 计算要加载的数据
        var itemsToLoad = new List<T>();

        if (_sourceData is IList list)
        {
            // 如果是 IList，直接按索引访问
            TotalCount = list.Count;
            var endIndex = Math.Min(startIndex + _cacheSize, TotalCount);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (list[i] is T item)
                {
                    // 更新 Index 属性（如果有的话），使其反映虚拟滚动中的实际行号
                    UpdateDisplayIndex(item, i);
                    itemsToLoad.Add(item);
                }
            }
        }
        else
        {
            // 如果是 IEnumerable，需要遍历
            var count = 0;
            var currentIndex = 0;

            var enumerator = _sourceData.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    count++;
                }
            }
            finally
            {
                (enumerator as IDisposable)?.Dispose();
            }

            TotalCount = count;
            var endIndex = Math.Min(startIndex + _cacheSize, TotalCount);

            if (startIndex >= 0 && startIndex < TotalCount)
            {
                currentIndex = 0;
                enumerator = _sourceData.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext() && currentIndex < endIndex)
                    {
                        if (currentIndex >= startIndex && enumerator.Current is T item)
                        {
                            // 更新 Index 属性（如果有的话）
                            UpdateDisplayIndex(item, currentIndex);
                            itemsToLoad.Add(item);
                        }
                        currentIndex++;
                    }
                }
                finally
                {
                    (enumerator as IDisposable)?.Dispose();
                }
            }
        }

        // 更新集合内容
        ReplaceRange(itemsToLoad);
    }

    /// <summary>
    /// 重置并重新加载 - 从位置 0 开始重新加载
    /// </summary>
    private void ResetAndReload()
    {
        _startIndex = 0;
        Reload(0);
    }

    /// <summary>
    /// 替换集合范围内容 - 避免频繁的集合变更通知
    /// </summary>
    private void ReplaceRange(List<T> newItems)
    {
        var oldItems = Items.ToList();

        // 如果内容完全相同，不做任何操作
        if (oldItems.Count == newItems.Count &&
            oldItems.SequenceEqual(newItems))
        {
            return;
        }

        // 替换所有项
        Items.Clear();
        foreach (var item in newItems)
        {
            Items.Add(item);
        }

        OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(
            System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
    }

    /// <summary>
    /// 获取指定索引的实际项目（从源数据中获取，不从缓存中获取）
    /// </summary>
    public T GetItemAt(int index)
    {
        if (_sourceData == null || index < 0 || index >= TotalCount)
        {
            return null;
        }

        if (_sourceData is IList list)
        {
            return list[index] as T;
        }
        else
        {
            var currentIndex = 0;
            var enumerator = _sourceData.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    if (currentIndex == index)
                    {
                        return enumerator.Current as T;
                    }
                    currentIndex++;
                }
            }
            finally
            {
                (enumerator as IDisposable)?.Dispose();
            }
        }

        return null;
    }

    /// <summary>
    /// 更新数据项的显示索引 - 使用反射动态更新 Index 属性
    /// </summary>
    private void UpdateDisplayIndex(T item, int displayIndex)
    {
        if (item == null)
            return;

        // 尝试通过反射查找并更新 Index 属性
        var indexProperty = item.GetType().GetProperty("Index");
        if (indexProperty != null && indexProperty.CanWrite)
        {
            try
            {
                indexProperty.SetValue(item, displayIndex + 1); // +1 因为显示的索引从 1 开始
            }
            catch
            {
                // 如果设置失败，忽略
            }
        }
    }
}
