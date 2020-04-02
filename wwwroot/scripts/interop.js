let OutsideClickDetectors = {};

//-----------------------------------------------------------------------------

function GetElementHierarchyIds(id, ids)
{
    let el = document.getElementById(id);

    if(el)
    {
        ids.push(id);

        let children     = el.childNodes;
        let num_children = children.length;

        for(let i = 0; i < num_children; ++i)
        {
            let child = children[i];

            if(child)
            {
                GetElementHierarchyIds(child.id, ids);
            }
        }
    }
}

//-----------------------------------------------------------------------------

class OutsideClickDetector
{
    constructor(id, obj, callback)
    {
        this.Id         = id;
        this.Obj        = obj;
        this.Callback   = callback;
        this.ClickFunc  = null;
        this.ElementIds = [];

        GetElementHierarchyIds(id, this.ElementIds);

        console.log("Element Hierarchy Ids");
        console.log(this.ElementIds);

        this.RegisterListener();
    }

    RegisterListener()
    {
        var instance = this;
        document.body.addEventListener("click", this.ClickFunc = function(e)
        {
            console.log("document.body click event handler");
            instance.OnBodyClick(e);
        }, false);
    }

    UnregisterListener()
    {
        document.body.removeEventListener('click', this.ClickFunc, false);
        console.log("UnregisterListener");
    }

    OnBodyClick(e)
    {
        let UseElementIds = false;

        if(UseElementIds)
        {
            let target_index = this.ElementIds.indexOf(e.target.id);

            console.log("OnBodyClick");
            console.log("Target Id");
            console.log(e.target.id);
            console.log("Target Index");
            console.log(target_index);

            if(e.target.id != this.Id)
            {
                if(target_index < 0)
                {
                    this.UnregisterListener();
                    this.Obj.invokeMethodAsync(this.Callback);
                }
            }
        }
        else
        {
            console.log("OnBodyClick");
            console.log("ThisId: " + this.Id);
            console.log("TaegetId: " + e.target.id);

            // This fails, even though an element with this id exists
            let el_this_id = document.getElementById(this.id);

            // This works, and the element it returns has our id
            // NOTE: querySelector with #id fails, just like getElementById
            let el_this_qu = document.querySelector(".PopUpColorPicker");

            // Target element works just fine
            let el_target  = document.getElementById(e.target.id);

            console.log("ThisElement (GetById)");
            console.log(el_this_id);
            console.log("ThisElement (QuerySelect)");
            console.log(el_this_qu);
            console.log("TargetElement");
            console.log(el_target);

            if(e.target.id != this.Id)
            {
                // Fails, because el_this_id is null
                //let contains = el_this_id.contains(el_target);

                // This works for a demo, but won't work in the real world
                // since we might have multiple instances of this component
                let contains = el_this_qu.contains(el_target);

                if(!contains)
                {
                    this.UnregisterListener();
                    this.Obj.invokeMethodAsync(this.Callback);
                }
            }
        }
    }
}

//-----------------------------------------------------------------------------

class OutsideClickManager
{
    static RegisterDetector(id, obj, callback)
    {
        let detector = OutsideClickDetectors[id];

        if(detector == null)
        {
            detector = new OutsideClickDetector(id, obj, callback);
            OutsideClickDetectors[id] = detector;
        }
        else
        {
            console.log("RegisterDetector: detector already registered");
        }
    }

    static UnregisterOutsideClickDetector(id)
    {
        let detector = OutsideClickDetectors[id];

        if(detector != null)
        {
            delete OutsideClickDetectors[id];
            OutsideClickDetectors[id] = null;
        }
    }
}

//-----------------------------------------------------------------------------

function RegisterOutsideClickDetector(id, obj, callback)
{
    OutsideClickManager.RegisterDetector(id, obj, callback);
}

//-----------------------------------------------------------------------------

function UnregisterOutsideClickDetector(id)
{
    OutsideClickManager.UnregisterOutsideClickDetector(id);
}

//-----------------------------------------------------------------------------

function GetBoundingClientRect(id)
{
    let json = "";

    let element = document.getElementById(id);

    if(element)
    {
        let rect = element.getBoundingClientRect();
        json = JSON.stringify(rect);
    }

    return json;
}

//-----------------------------------------------------------------------------

function SetCssStyleProperty(id, prop_name, prop_value)
{
    let element = document.getElementById(id);

    if(element)
    {
        element.style[prop_name] = prop_value;
    }
}

//-----------------------------------------------------------------------------

function GetElementValue(id)
{
    let val = "";

    let element = document.getElementById(id);

    if(element)
    {
        val = element.value;
    }

    return val;
}
