using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace Kenova.Client.Components
{
    public partial class TabComponent : KenovaComponentBase, IKenovaComponent
    {

        [JSInvokable]
        public void OnTabClicked(int index)
        {
            processTabClick(index);
        }

        [JSInvokable]
        public void OnEnterPressed(int index)
        {
            processTabClick(index);
        }


        [JSInvokable]
        public void OnSpacePressed(int index)
        {
            processTabClick(index);
        }

        private void processTabClick(int index)
        {
            TabItem item = this.TabItems[index];

            if (item.EnabledExpression() == false)
                return;

            this.SelectedTabFieldLink.Value = item.Identifier;

            this.StateHasChanged();

            _ = LayerComponent.PerformAutoFocusAsync(item.StagingAreaID);
        }

        public void Rerender()
        {
            this.StateHasChanged();
        }

        internal TabItem GetTabItem(string identifier)
        {
            return this.TabItems.FirstOrDefault(p => p.Identifier == identifier);
        }

        private int getTabIndex(string identifier)
        {
            for (int i = 0; i < this.TabItems.Count; i++)
            {
                if (this.TabItems[i].Identifier == identifier)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// If the SelectedTabFieldLink.Value is not an existing Identifier,
        /// then the Identifier value for the first tabpage will be returned.
        /// </summary>
        internal string SelectedIdentifierForTabPage()
        {
            int index = getTabIndex(this.SelectedTabFieldLink.Value);

            if (index == -1)
                index = 0;

            return this.TabItems[index].Identifier;
        }


        public async ValueTask SetFocusAsync()
        {
            int index = getTabIndex(this.SelectedTabFieldLink.Value);

            if (index == -1)
                index = 0;

            for (int i = 0; i < this.TabItems.Count; i++)
            {
                if (index == i)
                {
                    await JavaScriptCaller.KNFocusAsync(this.TabItems[i].ContainerID);
                    return;
                }
            }

        }

        async ValueTask<bool> IKenovaComponent.PerformFocusAsync(string focusID)
        {
            if (this.FocusID == null || focusID != this.FocusID)
                return false;

            int index = getTabIndex(this.SelectedTabFieldLink.Value);

            if (index == -1)
                index = 0;

            for (int i = 0; i < this.TabItems.Count; i++)
            {
                if (index == i)
                {
                    await JavaScriptCaller.KNFocusAsync(this.TabItems[i].ContainerID);
                    return true;
                }
            }

            return false;
        }

        async ValueTask IKenovaComponent.PerformAutoFocusAsync()
        {
            int index = getTabIndex(this.SelectedTabFieldLink.Value);

            if (index == -1)
                index = 0;

            for (int i = 0; i < this.TabItems.Count; i++)
            {
                if (index == i)
                {
                    await JavaScriptCaller.KNFocusAsync(this.TabItems[i].ContainerID);
                    return;
                }
            }
        }

        ValueTask<int> IKenovaComponent.MeasureAutoFocusPriorityAsync()
        {
            if (this.AutoFocus)
            {
                return ValueTask.FromResult(this.AutoFocusPriority);
            }

            return ValueTask.FromResult(-1);
        }

        ValueTask<bool> IKenovaComponent.PerformEnterPressedAsync()
        {
            return ValueTask.FromResult(false);
        }

        ValueTask<bool> IKenovaComponent.PerformEscapePressedAsync()
        {
            return ValueTask.FromResult(false);
        }

        ValueTask<bool> IKenovaComponent.ComponentHiddenAsync()
        {
            return JavaScriptCaller.KNElementHiddenAsync(container_id);
        }

        ValueTask<bool> IKenovaComponent.IsChildOfAsync(string parent_id)
        {
            return JavaScriptCaller.KNIsChildOfAsync(parent_id, container_id);
        }

    }
}