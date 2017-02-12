using XamForms.Controls;
using XamForms.Controls.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

[assembly: ExportRenderer(typeof(CalendarButton), typeof(CalendarButtonRenderer))]
namespace XamForms.Controls.Windows
{
    public class CalendarButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.MinWidth = 48;
            Control.MinHeight = 48;
            Control.Style = Calendar.CalendarButtonStyle;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = Element as CalendarButton;
            if (e.PropertyName == nameof(element.TextWithoutMeasure) || e.PropertyName == "Renderer")
            {
                Control.Content = element.TextWithoutMeasure;
            }

            if (e.PropertyName == nameof(element.BorderWidth) || e.PropertyName == "Renderer")
            {
                Control.BorderThickness = new Thickness(Element.BorderWidth);
            }
        }
    }

	public static class Calendar
	{
        public static Style CalendarButtonStyle { get; internal set; }
        public static void Init()
		{

#if WINDOWS_UWP
            CalendarButtonStyle = XamlReader.Load(
                @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                        xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" TargetType=""Button"" x:Key=""CalendarButtonStyle"">
                        <Setter Property=""Background"" Value=""{ThemeResource SystemControlBackgroundBaseLowBrush}""/>
                        <Setter Property=""Foreground"" Value=""{ThemeResource SystemControlForegroundBaseHighBrush}""/>
                        <Setter Property=""BorderBrush"" Value=""{ThemeResource SystemControlForegroundTransparentBrush}""/>
                        <Setter Property=""BorderThickness"" Value=""{ThemeResource ButtonBorderThemeThickness}""/>
                        <Setter Property=""Padding"" Value=""0""/>
                        <Setter Property=""HorizontalAlignment"" Value=""Left""/>
                        <Setter Property=""VerticalAlignment"" Value=""Center""/>
                        <Setter Property=""FontFamily"" Value=""{ThemeResource ContentControlThemeFontFamily}""/>
                        <Setter Property=""FontWeight"" Value=""Normal""/>
                        <Setter Property=""FontSize"" Value=""{ThemeResource ControlContentThemeFontSize}""/>
                        <Setter Property=""UseSystemFocusVisuals"" Value=""True""/>
                        <Setter Property=""Template"">
                            <Setter.Value>
                                <ControlTemplate TargetType=""Button"">
                                    <Grid x:Name=""RootGrid"" Background=""{TemplateBinding Background}"" Padding=""1"" Margin=""0"">
                                        <VisualStateManager.VisualStateGroups>
                                            <VisualStateGroup x:Name=""CommonStates"">
                                                <VisualState x:Name=""Normal"">
                                                    <Storyboard>
                                                        <PointerUpThemeAnimation Storyboard.TargetName=""RootGrid""/>
                                                    </Storyboard>
                                                </VisualState>
                                                <VisualState x:Name=""PointerOver"">
                                                    <Storyboard>
                                                        <ObjectAnimationUsingKeyFrames Storyboard.TargetProperty=""BorderBrush"" Storyboard.TargetName=""ContentPresenter"">
                                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""{ThemeResource SystemControlHighlightBaseMediumLowBrush}""/>
                                                        </ObjectAnimationUsingKeyFrames>
                                                        <ObjectAnimationUsingKeyFrames Storyboard.TargetProperty=""Foreground"" Storyboard.TargetName=""ContentPresenter"">
                                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""{ThemeResource SystemControlHighlightBaseHighBrush}""/>
                                                        </ObjectAnimationUsingKeyFrames>
                                                        <PointerUpThemeAnimation Storyboard.TargetName=""RootGrid""/>
                                                    </Storyboard>
                                                </VisualState>
                                                <VisualState x:Name=""Pressed"">
                                                    <Storyboard>
                                                        <PointerDownThemeAnimation Storyboard.TargetName=""RootGrid""/>
                                                    </Storyboard>
                                                </VisualState>
                                                <VisualState x:Name=""Disabled"">
                                                    <Storyboard>

                                                    </Storyboard>
                                                </VisualState>
                                            </VisualStateGroup>
                                        </VisualStateManager.VisualStateGroups>
                                        <ContentPresenter x:Name=""ContentPresenter"" AutomationProperties.AccessibilityView=""Raw"" BorderBrush=""{TemplateBinding BorderBrush}"" BorderThickness=""{TemplateBinding BorderThickness}"" ContentTemplate=""{TemplateBinding ContentTemplate}"" ContentTransitions=""{TemplateBinding ContentTransitions}"" Content=""{TemplateBinding Content}"" HorizontalContentAlignment=""{TemplateBinding HorizontalContentAlignment}"" Padding=""{TemplateBinding Padding}"" VerticalContentAlignment=""{TemplateBinding VerticalContentAlignment}""/>
                                    </Grid>
                                </ControlTemplate>
                            </Setter.Value>
                        </Setter>
                    </Style>") as Style;
#else

            CalendarButtonStyle = XamlReader.Load(
                @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" x:Key=""CalendarButtonStyle"" TargetType=""Button"">
                    <Setter Property=""Background"" Value=""{ThemeResource ButtonBackgroundThemeBrush}""/>
                    <Setter Property=""Foreground"" Value=""{ThemeResource ButtonForegroundThemeBrush}""/>
                    <Setter Property=""BorderBrush"" Value=""{ThemeResource ButtonBorderThemeBrush}""/>
                    <Setter Property=""BorderThickness"" Value=""{ThemeResource ButtonBorderThemeThickness}""/>
                    <Setter Property=""Padding"" Value=""0""/>
                    <Setter Property=""HorizontalAlignment"" Value=""Center""/>
                    <Setter Property=""VerticalAlignment"" Value=""Center""/>
                    <Setter Property=""FontFamily"" Value=""{ThemeResource ContentControlThemeFontFamily}""/>
                    <Setter Property=""FontWeight"" Value=""SemiBold""/>
                    <Setter Property=""FontSize"" Value=""{ThemeResource ControlContentThemeFontSize}""/>
                    <Setter Property=""Template"">
                        <Setter.Value>
                            <ControlTemplate TargetType=""Button"">
                                <Grid Padding=""1"" Margin=""0"">
                                    <VisualStateManager.VisualStateGroups>
                                        <VisualStateGroup x:Name=""CommonStates"">
                                            <VisualState x:Name=""Normal""/>
                                            <VisualState x:Name=""PointerOver"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Storyboard.TargetProperty=""BorderBrush"" Storyboard.TargetName=""Border"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Black""/>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </VisualState>
                                            <VisualState x:Name=""Pressed"" />
                                            <VisualState x:Name=""Disabled"" />
                                        </VisualStateGroup>
                                        <VisualStateGroup x:Name=""FocusStates"">
                                            <VisualState x:Name=""Focused"" />
                                            <VisualState x:Name=""Unfocused""/>
                                            <VisualState x:Name=""PointerFocused""/>
                                        </VisualStateGroup>
                                    </VisualStateManager.VisualStateGroups>
                                    <Border x:Name=""Border"" BorderBrush=""{TemplateBinding BorderBrush}"" BorderThickness=""{TemplateBinding BorderThickness}"" Background=""{TemplateBinding Background}"" Margin=""0,0,0,0"">
                                        <ContentPresenter x:Name=""ContentPresenter"" AutomationProperties.AccessibilityView=""Raw"" ContentTemplate=""{TemplateBinding ContentTemplate}"" ContentTransitions=""{TemplateBinding ContentTransitions}"" Content=""{TemplateBinding Content}"" HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}"" Margin=""0"" VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""/>
                                    </Border>
                                    <Rectangle x:Name=""FocusVisualWhite"" IsHitTestVisible=""False"" Opacity=""0"" StrokeDashOffset=""1.5"" StrokeEndLineCap=""Square"" Stroke=""{ThemeResource FocusVisualWhiteStrokeThemeBrush}"" StrokeDashArray=""1,1""/>
                                    <Rectangle x:Name=""FocusVisualBlack"" IsHitTestVisible=""False"" Opacity=""0"" StrokeDashOffset=""0.5"" StrokeEndLineCap=""Square"" Stroke=""{ThemeResource FocusVisualBlackStrokeThemeBrush}"" StrokeDashArray=""1,1""/>
                                </Grid>
                            </ControlTemplate>
                        </Setter.Value>
                    </Setter>
                </Style>") as Style;
#endif
        }
    }
}
