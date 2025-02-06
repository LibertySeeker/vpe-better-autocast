using RimWorld;
using UnityEngine;
using Verse;

namespace BetterAutocastVPE;

public class Area_LargeSolarPinhole : Area
{
    public override string Label => "BetterAutocastVPE.LargeSolarPinholeArea".TranslateSafe();

    public override Color Color => new(0.97f, 0.84f, 0.11f);

    public override int ListPriority => 1000;

    public Area_LargeSolarPinhole() { }

    public Area_LargeSolarPinhole(AreaManager areaManager)
        : base(areaManager) { }

    public override string GetUniqueLoadID()
    {
        return "Area_" + ID + "_LargeSolarPinhole";
    }
}

public class Designator_Area_LargeSolarPinhole : Designator_Cells
{
    private readonly DesignateMode mode;

    public override bool Visible => BetterAutocastVPE.Settings.ShowLargeSolarPinholeArea;

    public override int DraggableDimensions => 2;

    public override bool DragDrawMeasurements => true;

    protected Designator_Area_LargeSolarPinhole(DesignateMode mode)
    {
        this.mode = mode;
        soundDragSustain = SoundDefOf.Designate_DragStandard;
        soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
        useMouseIcon = true;
    }

    public override AcceptanceReport CanDesignateCell(IntVec3 c)
    {
        if (!c.InBounds(Map))
        {
            return false;
        }
        bool cellContained = Map.areaManager.Get<Area_LargeSolarPinhole>()[c];
        return mode switch
        {
            DesignateMode.Add => !cellContained,
            DesignateMode.Remove => cellContained,
            _ => throw new System.NotImplementedException(),
        };
    }

    public override void DesignateSingleCell(IntVec3 c)
    {
        Map.areaManager.Get<Area_LargeSolarPinhole>()[c] = mode == DesignateMode.Add;
    }

    public override void SelectedUpdate()
    {
        GenUI.RenderMouseoverBracket();
        Map.areaManager.Get<Area_LargeSolarPinhole>().MarkForDraw();
    }
}

public class Designator_Area_LargeSolarPinhole_Expand : Designator_Area_LargeSolarPinhole
{
    public Designator_Area_LargeSolarPinhole_Expand()
        : base(DesignateMode.Add)
    {
        defaultLabel = "BetterAutocastVPE.LargeSolarPinholeArea.Expand".TranslateSafe();
        defaultDesc = "BetterAutocastVPE.LargeSolarPinholeArea.Expand.Description".TranslateSafe();
        icon = ContentFinder<Texture2D>.Get("UI/Icons/BetterAutocastVPE/LargeSolarPinholeArea");
        soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
        soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
        soundSucceeded = SoundDefOf.Designate_ZoneAdd_Stockpile;
    }
}

public class Designator_Area_LargeSolarPinhole_Clear : Designator_Area_LargeSolarPinhole
{
    public Designator_Area_LargeSolarPinhole_Clear()
        : base(DesignateMode.Remove)
    {
        defaultLabel = "BetterAutocastVPE.LargeSolarPinholeArea.Remove".TranslateSafe();
        defaultDesc = "BetterAutocastVPE.LargeSolarPinholeArea.Remove.Description".TranslateSafe();
        icon = ContentFinder<Texture2D>.Get("UI/Icons/BetterAutocastVPE/LargeSolarPinholeAreaOff");
        soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
        soundDragChanged = null;
        soundSucceeded = SoundDefOf.Designate_ZoneDelete;
    }
}
