# BlazorGetElementByIdBug
 Blazor project where javascript's getElementById fails

## Project Overview

This is a simplified project that demonstrates a problem I ran into on a larger project. I am trying to create a color picker component that contains two parts:

* A colored box that displays the current color
* A floating panel with controls to edit the color values

The workflow is as follows

* The floating panel is hidden most of the time
* If you click the color box the floating editor panel pops up below the box
* If you click on the panel, or any of the editing fields, the panel remains visible
* If you click anywhere outside of the panel it becomes hidden

## How It Works

To achieve this behavior requires some javascript interop. Specifically:

* When the panel pops up an event listener is added to the document body, to detect any click that may haappen.
* When a click occurs, it checks whether the click target is our panel, or any of the elements within it.
* If so, nothing happens
* If not, the body click handler is removed, and the panel is hidden

## The Bug

For some reason, getElementById returns null when passed the floating panel id. It works fine for all other elements, including child elements of the panel.

Interestingly, if we use querySelector instead and search by class, it does find the panel. And the element it returns has an id that matches the id we tried to use with getElementById.

Unfortunately, searching by class isn't sufficient for real world use cases, since we may have multiple instances of this component on the same page. But for this demo project I'm using that method to demonstrate how it should work.

You can find the relevant code in interop.js, in the OnBodyClick function. The key lines of code to inspect are

```javascript
// This fails, even though an element with this id exists
let el_this_id = document.getElementById(this.id);

// This works, and the element it returns has our id
// NOTE: querySelector with #id fails, just like getElementById
let el_this_qu = document.querySelector(".PopUpColorPicker");

// Target element works just fine
let el_target  = document.getElementById(e.target.id);
```

## Notes

Debug logging code has been added, so if you run the project and open up the debug console, you can easily see the values being used and returned.

This bug is consistent across Chrome, Firefox, and Edge. This makes me suspect it may be caused by something in the Blazor rendering process.

## Update

I was able to come up with a workaround that I can live with, but I don't think it's ideal. Here's what I did:

### ColorSwatchBase.razor.cs

1. Moved the call to register the click event from TogglePickerVisibility to OnAfterRenderAsync, when the panel is first shown.

### interop.js

1. Record the ids of the popup, and all children, when the click event is registered
NOTE: getElementById works fine here.

2. Use those ids in OnBodyClick, instead of element contains.

I've left the old method that uses element contains and you can easily toggle between these two methods by changing UseElementIds in OnBodyClick.

## Technical Details

* Visual Studio 16.5.1
* Blazor WebAssembly
* ASP.NET Core 3.1

## Additional minor problem

When you launch the project, it says "An unhandled error has occurred". It doesn't seem to affect anything, but I'm curious what's causing that.

