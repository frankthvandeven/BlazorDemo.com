﻿using BlazorDemo.Client.VenturaAutoCreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorDemo.Client.Components
{
    public partial class OverlayTester : LayerComponentBase
    {

        private Button BtnOpen;

        private string Mode = "ModalFullsize";

        [Parameter]
        public int Number { get; set; } = 0;


        protected override void OnLayerInitialized()
        {
            Breadcrumb = $"Overlay{Number + 1}";

        }


        protected override void OnDispose()
        {
            Console.WriteLine($"OVERLAYTESTER DISPOSE Overlay{Number}");
        }

        protected override void OnAfterRender(bool firstRender)
        {
            //if (firstRender && Number > 0)
            //{
            //    this.SetBreadCrumb($"New Crumb{Number}");
            //}

            base.OnAfterRender(firstRender);
        }

        private void OpenOverlay()
        {
            LayerDefinition<OverlayTester> ld;

            if (Mode == "Modal")
            {
                ld = new LayerDefinition<OverlayTester>
                {
                    Kind = LayerKind.ModalWindow,
                    [i => i.Number] = Number + 1
                };

                ld.Open();
            }
            else if (Mode == "ModalFullsize")
            {
                ld = new LayerDefinition<OverlayTester>
                {
                    Kind = LayerKind.Modal,
                    [i => i.Number] = Number + 1
                };

                ld.Open();
            }
            else if (Mode == "ModelessRight")
            {
                ld = new LayerDefinition<OverlayTester>
                {
                    Kind = LayerKind.ModelessRight,
                    [i => i.Number] = Number + 1
                };

                ld.Open();
            }
            else if (Mode == "Dropdown")
            {
                ld = new LayerDefinition<OverlayTester>
                {
                    Kind = LayerKind.Dropdown,
                    OwnerID = BtnOpen.ContainerID,
                    [i => i.Number] = Number + 1
                };

                ld.Open();
            }
            else if (Mode == "DropdownBalloon")
            {
                ld = new LayerDefinition<OverlayTester>
                {
                    Kind = LayerKind.DropdownBalloon,
                    OwnerID = BtnOpen.ContainerID,
                    [i => i.Number] = Number + 1
                };
                    
                ld.Open();
            }

        }


    }
}