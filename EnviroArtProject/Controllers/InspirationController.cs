using System.Text.Json;
using System.Xml.Linq;
using EnviroArtProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Unicode;
using System.Text;
using System.Drawing;

namespace EnviroArtProject.Controllers;

public class InspirationController : Controller
{
    private readonly HttpClient _http;

    public InspirationController(IHttpClientFactory httpFactory)
    {
        _http = httpFactory.CreateClient();
    }

    [HttpGet]
    public IActionResult EnviroIndex()
    {
        var worldIdeaModel = new WorldIdea(); // empty model
        return View("EnviroIndex", worldIdeaModel);  
    }
    
    public async Task<IActionResult> GenerateEnviroIdea()
    {
        var worldIdeaModel = new WorldIdea();

        worldIdeaModel.WorldLocation = GetWorldLocation();
        worldIdeaModel.Colors = await GetColorPalette() ?? new ColorPalette(); // in case of an empty reply from api
        worldIdeaModel.WordsInfos = await GetWordList() ?? new Words();

        return View("EnviroIndex", worldIdeaModel);
    }
    
    private WorldLocation GetWorldLocation()
    {
        var worldLocation = new WorldLocation();
        var image = "https://picsum.photos/200";
        worldLocation.Url = image;
        
        return worldLocation;
    }

    public async Task<ColorPalette?> GetColorPalette()
    {
        var colorPalette = new ColorPalette();

        var url = "http://colormind.io/api/";

        // colormind asks for a POST
        var data = new
        {
            model = "default"
        };

        var jsonBody = JsonSerializer.Serialize(data);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
            return null;

        // trim
        var json = await response.Content.ReadAsByteArrayAsync(); 
        var palette = JsonSerializer.Deserialize<ColorApiResponse>(json);

        colorPalette.Palette = palette?.result;

        return colorPalette;
    }

    public async Task<Words?> GetWordList()
    {
        var words = new Words();

        var response = await _http.GetStringAsync("https://random-word-api.vercel.app/api?words=2&type=capitalized");

        // dto is not used since this api response is just a simple json
        var wordsData = JsonSerializer.Deserialize<string[]>(response);

        words.WordArray = wordsData;
        
        return words;
    }
}