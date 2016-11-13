using NetCarto.CartoCSS.Attributes;

namespace NetCarto.CartoCSS
{
    public enum Symbolyzer
    {
        All = ~0,
        [CartoCssSymbol("polygon")]
        Polygon = 1,            // (polygon)
        [CartoCssSymbol("line")]
        Line = 2,               // (lines and polygons)
        [CartoCssSymbol("line-pattern")]
        Markers = 4,            // (points, lines, and polygons)
        [CartoCssSymbol("shield")]
        Shield = 8,
        [CartoCssSymbol("line-pattern")]
        LinePattern = 16,       //(lines and polygons)	
        [CartoCssSymbol("polygon-pattern")]
        PolygonPattern = 32,    //(polygons)
        [CartoCssSymbol("raster")]
        Raster = 64,            //(grid data layers)	
        [CartoCssSymbol("point")]
        Point = 128,            //(points)	
        [CartoCssSymbol("text")]
        Text = 256,             //Text (points, lines, and polygons)
        [CartoCssSymbol("building")]
        Building = 512,        // Buildings
        Ignore = 2048
    }
    
    [CartoCssProperty("comp-op")]
    public enum Composite
    {
        None,
        [CartoCSSValue("clear", Symbolyzer.All)]
        Clear,
        [CartoCSSValue("src", Symbolyzer.All)]
        Src,
        [CartoCSSValue("dst", Symbolyzer.All)]
        Dst,
        [CartoCSSValue("src-over", Symbolyzer.All)]
        SrcOver,
        [CartoCSSValue("dst-over", Symbolyzer.All)]
        DstOver,
        [CartoCSSValue("src-in", Symbolyzer.All)]
        SrcIn,
        [CartoCSSValue("dst-in", Symbolyzer.All)]
        DstIn,
        [CartoCSSValue("src-out", Symbolyzer.All)]
        SrcOut,
        [CartoCSSValue("dst-out", Symbolyzer.All)]
        DstOut,
        [CartoCSSValue("src-atop", Symbolyzer.All)]
        SrcAtop,
        [CartoCSSValue("dst-atop", Symbolyzer.All)]
        DstAtop,
        [CartoCSSValue("xor", Symbolyzer.All)]
        Xor,
        [CartoCSSValue("plus", Symbolyzer.All)]
        Plus,
        [CartoCSSValue("minus", Symbolyzer.All)]
        Minus,
        [CartoCSSValue("multiply", Symbolyzer.All)]
        Multiply,
        [CartoCSSValue("screen", Symbolyzer.All)]
        Screen,
        [CartoCSSValue("overlay", Symbolyzer.All)]
        Overlay,
        [CartoCSSValue("darken", Symbolyzer.All)]
        Darken,
        [CartoCSSValue("lighten", Symbolyzer.All)]
        Lighten,
        [CartoCSSValue("color-dodge", Symbolyzer.All)]
        ColorDodge,
        [CartoCSSValue("color-burn", Symbolyzer.All)]
        ColorBurn,
        [CartoCSSValue("hard-light", Symbolyzer.All)]
        HardLight,
        [CartoCSSValue("soft-light", Symbolyzer.All)]
        SoftLight,
        [CartoCSSValue("difference", Symbolyzer.All)]
        Difference,
        [CartoCSSValue("exclusion", Symbolyzer.All)]
        Exclusion,
        [CartoCSSValue("contrast", Symbolyzer.All)]
        Contrast,
        [CartoCSSValue("invert", Symbolyzer.All)]
        Invert,
        [CartoCSSValue("invert-rgb", Symbolyzer.All)]
        InvertRGB,
        [CartoCSSValue("grain-merge", Symbolyzer.All)]
        GrainMerge,
        [CartoCSSValue("grain-extract", Symbolyzer.All)]
        GrainExtract,
        [CartoCSSValue("hue", Symbolyzer.All)]
        HUE,
        [CartoCSSValue("saturation", Symbolyzer.All)]
        Saturation,
        [CartoCSSValue("color", Symbolyzer.All)]
        Color,
        [CartoCSSValue("value", Symbolyzer.All)]
        Value,
    }

    [CartoCssProperty("image-filters")]
    public enum ImageFilter
    {
        [CartoCSSValue("none", Symbolyzer.Ignore)]
        None,
        [CartoCssFunction("colorize-alpha({0})")]
        ColorizeAlpha
    }

    [CartoCssProperty("background-image")]
    public enum Background
    {
        [CartoCssFunction("uri({0})")]
        Uri,

    }
}
