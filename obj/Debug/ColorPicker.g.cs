﻿#pragma checksum "..\..\ColorPicker.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6434FF25BABB5C5DE54053BB5E13837B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace WPF_LED_Controller {
    
    
    /// <summary>
    /// ColorPicker
    /// </summary>
    public partial class ColorPicker : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\ColorPicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WPF_LED_Controller.ColorPicker cpcColor;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\ColorPicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image image;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\ColorPicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas CanColor;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\ColorPicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse EpPointer;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WPF LED Controller;component/colorpicker.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ColorPicker.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cpcColor = ((WPF_LED_Controller.ColorPicker)(target));
            return;
            case 2:
            this.image = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.CanColor = ((System.Windows.Controls.Canvas)(target));
            
            #line 16 "..\..\ColorPicker.xaml"
            this.CanColor.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.CanColor_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 16 "..\..\ColorPicker.xaml"
            this.CanColor.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.CanColor_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 16 "..\..\ColorPicker.xaml"
            this.CanColor.MouseMove += new System.Windows.Input.MouseEventHandler(this.CanColor_MouseMove);
            
            #line default
            #line hidden
            return;
            case 4:
            this.EpPointer = ((System.Windows.Shapes.Ellipse)(target));
            
            #line 21 "..\..\ColorPicker.xaml"
            this.EpPointer.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.EpPointer_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 21 "..\..\ColorPicker.xaml"
            this.EpPointer.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.EpPointer_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 21 "..\..\ColorPicker.xaml"
            this.EpPointer.MouseMove += new System.Windows.Input.MouseEventHandler(this.EpPointer_MouseMove);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

