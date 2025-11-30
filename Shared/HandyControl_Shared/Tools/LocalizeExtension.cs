
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace HandyControl;

public class LocalizeExtension : MarkupExtension
{
    private DependencyObject targetObject;
    private DependencyProperty targetProperty;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizeExtension" /> class.
    /// </summary>
    public LocalizeExtension()
    {
        //LocalizationManager.CultureChanged += LocalizationManager_CultureChanged;
    }

    

    public LocalizeExtension(string text)
    {
        Text = text;
    }   
 

    /// <summary>
    /// 文本
    /// </summary>
    public string Text { get; set; }

  

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (targetObject == null)
        {
            var targetHelper = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            targetObject = targetHelper.TargetObject as DependencyObject;
            targetProperty = targetHelper.TargetProperty as DependencyProperty;
        }
 
        return Text.ToLanguage( );
    }

    private void LocalizationManager_CultureChanged(object sender, EventArgs e)
    {
        if (targetObject != null && targetProperty != null)
        { 
            targetObject.SetValue(targetProperty, Text.ToLanguage());
        }
    }
}