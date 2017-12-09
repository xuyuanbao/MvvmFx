﻿using System.ComponentModel;
using BoundControls.Business;
using MvvmFx.Windows.Data;
using Binding = MvvmFx.Windows.Data.Binding;
using Menu = BoundControls.Business.Menu;
#if WISEJ
using MvvmFx.WisejWeb;
using Wisej.Web;
using ToolStrip = Wisej.Web.MenuBar;
using ToolStripDropDownItem = Wisej.Web.MenuItem;
using ToolStripItem = Wisej.Web.MenuItem;
#else
using MvvmFx.Windows.Forms;
using System.Windows.Forms;
#endif

namespace MvvmFx.CaliburnMicro
{
    public class MvvmFxBindComponents
    {
        private readonly BindingManager _bindingManager = new BindingManager();

        public void SetMvvmFxBindings(Control masterControl)
        {
            _bindingManager.Bindings.Clear();

            foreach (Control control in masterControl.Controls)
            {
                if (control is ToolStrip)
                {
                    ParseComponents(control as ToolStrip);
                }
            }
        }

        private void ParseComponents(ToolStrip toolStrip)
        {
#if WISEJ
            foreach (ToolStripItem item in toolStrip.MenuItems)
#else
            foreach (ToolStripItem item in toolStrip.Items)
#endif
            {
                TryBind(item);
                RecurseToolStripItem(item);
            }
        }

        private void RecurseToolStripItem(ToolStripItem toolStripItem)
        {
            if (toolStripItem is ToolStripDropDownItem)
            {
#if WISEJ
                foreach (ToolStripItem item in ((ToolStripDropDownItem) toolStripItem).MenuItems)
#else
                foreach (ToolStripItem item in ((ToolStripDropDownItem) toolStripItem).DropDownItems)
#endif
                {
                    TryBind(item);
                    RecurseToolStripItem(item);
                }
            }
        }

        private void TryBind(Component component)
        {
            if (component is INamedBindable)
            {
                var namedBindable = component as INamedBindable;
                var menu = MenuCollection.GetMenu(namedBindable);
                Bind(namedBindable, menu);
            }
        }

        private void Bind(IBindableComponent bindable, Menu menu)
        {
            _bindingManager.Bindings.Add(new Binding(bindable, "Text", menu, "Text")
            {
                Mode = BindingMode.OneWayToTarget
            });
#if !WISEJ
            _bindingManager.Bindings.Add(new Binding(bindable, "ToolTipText", menu, "ToolTipText")
            {
                Mode = BindingMode.OneWayToTarget
            });
#endif
            _bindingManager.Bindings.Add(new Binding(bindable, "Enabled", menu, "Enabled")
            {
                Mode = BindingMode.OneWayToTarget
            });
            _bindingManager.Bindings.Add(new Binding(bindable, "Visible", menu, "Visible")
            {
                Mode = BindingMode.OneWayToTarget
            });
        }
    }
}