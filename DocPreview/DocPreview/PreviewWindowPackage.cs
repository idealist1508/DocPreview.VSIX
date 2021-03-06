﻿//------------------------------------------------------------------------------
// <copyright file="PreviewWindowPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System.Reflection;

namespace DocPreview
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(PreviewWindow))]
    [Guid(PreviewWindowPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class PreviewWindowPackage : Package
    {
        /// <summary>
        /// PreviewWindowPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "e251fb80-a496-4fcf-94c3-f835d61e917a";

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewWindow"/> class.
        /// </summary>
        public PreviewWindowPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve; 

            //var textEditorEvents = Global.GetDTE2().Events.TextEditorEvents;
            //textEditorEvents.LineChanged += TextEditorEvents_LineChanged;

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick; 
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            OnLineChanged?.Invoke();
        }

        //http://stackoverflow.com/questions/16557923/from-a-vs2008-vspackage-how-do-i-get-notified-whenever-caret-position-changed
        //private void TextEditorEvents_LineChanged(EnvDTE.TextPoint StartPoint, EnvDTE.TextPoint EndPoint, int Hint)
       // {
        //}

        static public Action OnLineChanged;

        //Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    if (args.Name.StartsWith("DocPrevGen"))
        //        return Assembly.Load(DocPreview.Resources.Resource1.DocPrevGen);
        //    else
        //        return null;
        //}

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            PreviewWindowCommand.Initialize(this);
            base.Initialize();
        }

        #endregion
    }
}
