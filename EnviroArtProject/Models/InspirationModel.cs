namespace EnviroArtProject.Models;

public class WorldIdea
{
    public WorldLocation WorldLocation {get; set;} = new WorldLocation();
    public ColorPalette Colors {get; set;} = new ColorPalette();
    public Words WordsInfos {get; set;} = new Words();
}

public class WorldLocation
{
    public string? Url {get; set;}
}

public class ColorPalette
{
    // array of array of int for rgb values [[432, 234, 54], ]
    public int[][]? Palette {get;set;}

}

public class Words
{
    public string[]? WordArray {get; set;}

    // definition is yet to be used
    public string[]? DefinitionArray {get; set;}
}

