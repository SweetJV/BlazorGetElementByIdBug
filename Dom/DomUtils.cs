using BlazorGetElementByIdBug.Dom;
using BlazorGetElementByIdBug.Values;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGetElementByIdBug.Dom
{
    public class DomUtils
    {
        public static async Task<DomRect> GetBoundingClientRect(IJSRuntime JS, string id)
        {
            string json = await JS.InvokeAsync<string>("GetBoundingClientRect", id);

            DomRect rect = JsonConvert.DeserializeObject<DomRect>(json);
            return rect;
        }

        public static async Task<Vector2> GetLocalMousePosition(IJSRuntime JS, string id, MouseEventArgs e)
        {
            DomRect rect = await GetBoundingClientRect(JS, id);

            Vector2 pos = new Vector2();
            pos.X = (float)(e.ClientX - rect.left);
            pos.Y = (float)(e.ClientY - rect.top);

            return pos;
        }

        public static async Task SetCssStyleProperty(IJSRuntime JS, string id, string prop_name, string prop_value)
        {
            await JS.InvokeAsync<object>("SetCssStyleProperty", id, prop_name, prop_value);
        }
    }
}
