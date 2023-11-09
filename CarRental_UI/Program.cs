using CarRental.Business.Classes;
using CarRental.Data.Classes;
using CarRental.Data.Interfaces;
using CarRental_UI;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Används för att läsa JSON-fil, är automatisk med
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// My services
// Add Business-class
builder.Services.AddSingleton<BookingProcessor>();
// Which use IData as input -> Most inject that also
// then runtime can deliver that object
// Also need to specify which class to implement
builder.Services.AddSingleton<IData, CollectionData>();

await builder.Build().RunAsync();
