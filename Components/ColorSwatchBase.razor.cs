﻿using BlazorGetElementByIdBug.Dom;
using BlazorGetElementByIdBug.Utils;
using BlazorGetElementByIdBug.Values;
using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorGetElementByIdBug.Components
{
    public class ColorSwatchBase : ComponentBase, IDisposable
    {
        [Inject]
        IJSRuntime JS { get; set; }

        private Color m_color = new Color(255, 255, 255);

        public Color Color
        {
            get { return m_color; }
            set
            {
                if(m_color != value)
                {
                    m_color = value;
                }
            }
        }

        protected string SwatchId = Guid.NewGuid().ToString();
        protected string PickerId = Guid.NewGuid().ToString();

        protected string RedId   = Guid.NewGuid().ToString();
        protected string GreenId = Guid.NewGuid().ToString();
        protected string BlueId  = Guid.NewGuid().ToString();
        protected string AlphaId = Guid.NewGuid().ToString();

        protected string RedDivId   = Guid.NewGuid().ToString();
        protected string GreenDivId = Guid.NewGuid().ToString();
        protected string BlueDivId  = Guid.NewGuid().ToString();
        protected string AlphaDivId = Guid.NewGuid().ToString();

        protected string RedLabelId   = Guid.NewGuid().ToString();
        protected string GreenLabelId = Guid.NewGuid().ToString();
        protected string BlueLabelId  = Guid.NewGuid().ToString();
        protected string AlphaLabelId = Guid.NewGuid().ToString();

        protected string PickerStyle;

        protected bool ShowPicker;
        protected bool UpdatingPicker;

        protected DotNetObjectReference<ColorSwatchBase> m_net_obj;

        protected override async Task OnAfterRenderAsync(bool first_render)
        {
            if(first_render)
            {
                await UpdateColorSwatch();
            }

            if(!UpdatingPicker)
            {
                // For some reason, it takes two rounds of updates
                // to get the controls to line up. So, we set a flag
                UpdatingPicker = true;

                DomRect swatch_rect = await DomUtils.GetBoundingClientRect(JS, SwatchId);

                float pick_x = swatch_rect.left;
                float pick_y = swatch_rect.top + 50;

                PickerStyle = "background-color: #444;";
                PickerStyle += "width: 200px; height: 200px;";
                PickerStyle += "position: fixed;";
                PickerStyle += $"left: {pick_x}px;";
                PickerStyle += $"top: {pick_y}px;";

                StateHasChanged();
            }
            else
            {
                UpdatingPicker = false;
            }
        }

        protected async Task UpdateColorSwatch()
        {
            string hex_color = ValueUtils.ColorToHexColor(Color);
            await DomUtils.SetCssStyleProperty(JS, SwatchId, "background-color", hex_color);
        }

        protected async Task TogglePickerVisibility()
        {
            ShowPicker = !ShowPicker;

            if(ShowPicker)
            {
                if(m_net_obj == null)
                {
                    m_net_obj = DotNetObjectReference.Create(this);
                    await JS.InvokeAsync<object>("RegisterOutsideClickDetector", PickerId, m_net_obj, "OnOutsideClick");
                }
            }
            else
            {
                if(m_net_obj != null)
                {
                    await JS.InvokeAsync<object>("UnregisterOutsideClickDetector", PickerId);
                    m_net_obj = null;
                }
            }

            StateHasChanged();
        }

        protected async Task OnSwatchClick(MouseEventArgs e)
        {
            await TogglePickerVisibility();
        }

        [JSInvokable]
        public async Task OnOutsideClick()
        {
            await TogglePickerVisibility();
        }

        protected async Task ColorPickerChange(ChangeEventArgs e)
        {
            string sr = await JS.InvokeAsync<string>("GetElementValue", RedId);
            string sg = await JS.InvokeAsync<string>("GetElementValue", GreenId);
            string sb = await JS.InvokeAsync<string>("GetElementValue", BlueId);
            string sa = await JS.InvokeAsync<string>("GetElementValue", AlphaId);

            byte r, g, b, a;
            byte.TryParse(sr, out r);
            byte.TryParse(sg, out g);
            byte.TryParse(sb, out b);
            byte.TryParse(sa, out a);

            Color c = new Color(r, g, b, a);
            this.Color = c;

            await UpdateColorSwatch();
        }

        protected async Task Log(string message)
        {
            await JS.InvokeAsync<object>("console.log", message);
        }

        public void Dispose()
        {
            m_net_obj?.Dispose();
        }
    }
}