using Microsoft.AspNetCore.Mvc;
using ClientSide.Models;

namespace ClientSide.Controllers;

public class HomeController : Controller
{
    private IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetByIdAsync()
    {
        try
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:7171");
            HttpResponseMessage response = httpClient.GetAsync("https://localhost:7171/api/store/1").Result;
            if (response.IsSuccessStatusCode)
            {
                GroceryStore grocery = await response.Content.ReadAsAsync<GroceryStore>();
                return View(grocery);
            }
            else
            {
                return Content("An error has occurred");
            }
        }
        catch (Exception e)
        {
            var error = e.GetBaseException();
            return Content("An error has occurred");
        }
    }

    //https://localhost:7247/Home/Post
    public async Task<IActionResult> PostAsync()
    {
        HttpClient httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("http://localhost:7171");
        GroceryStore newGrocery = new GroceryStore { Name = "Martin General Stores", Address = "4160  Oakwood Avenue" };
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("https://localhost:7171/api/store", newGrocery);
        if (response.IsSuccessStatusCode)
        {
            GroceryStore grocery = await response.Content.ReadAsAsync<GroceryStore>();
            return View("GetById", grocery);
        }
        else
        {
            return Content("An error has occurred");
        }
    }
}