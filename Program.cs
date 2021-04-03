using ConsoleClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAllProducts().Wait();
            Console.WriteLine("Enter ProductId");
            int productid = int.Parse(Console.ReadLine());
            GetProductById(productid).Wait();
            Product product = new Product();
            Console.WriteLine("Enter ID");
            product.productId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Name");
            product.Name = Console.ReadLine();
            Console.WriteLine("Enter Quantity in Stock");
            product.QtyInStock = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Description");
            product.Description = Console.ReadLine();
            Console.WriteLine("Enter Supplier");
            product.Supplier = Console.ReadLine();

            Insert(product).Wait();
            GetAllProducts().Wait();

            Put().Wait();
            GetAllProducts().Wait();

            Delete().Wait();
            GetAllProducts().Wait();
        }
        static async Task GetAllProducts()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44375/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Api/Products");
                if (response.IsSuccessStatusCode)
                {

                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    var productList = JsonConvert.DeserializeObject<List<Product>>(jsonString.Result);

                    foreach (var temp in productList)
                    {
                        Console.WriteLine("Id:{0}\tName:{1}", temp.productId, temp.Name);

                    }

                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine("Internal server Error");
                }
            }
        }
        static async Task GetProductById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44375/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("Api/Products/" + id);
                if (response.IsSuccessStatusCode)
                {
                    Product product = await response.Content.ReadAsAsync<Product>();
                    Console.WriteLine("Id:{0}\tName:{1}", product.productId, product.Name);

                }
                else
                {
                    Console.WriteLine(response.StatusCode);

                }

            }
        }
        static async Task Insert(Product product)
        {
            using (var client = new HttpClient())
            {
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("https://localhost:44375/");


                HttpResponseMessage response = await client.PostAsJsonAsync("Api/Products/", product);

                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.  
                    Console.WriteLine(response.StatusCode);
                }
            }
        }
        static async Task Put()
        {

            using (var client = new HttpClient())
            {
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("https://localhost:44375/");

                //PUT Method  
                var department = new Product() { productId = 9, Name = "Updated Product" };
                int id = 1;
                HttpResponseMessage response = await client.PutAsJsonAsync("Api/Products/" + id, department);
                if (response.IsSuccessStatusCode)

                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                }
            }
        }
        static async Task Delete()
        {
            using (var client = new HttpClient())
            {
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("https://localhost:44375/");


                int id = 1;
                HttpResponseMessage response = await client.DeleteAsync("Api/Products/" + id);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                    Console.WriteLine(response.StatusCode);
            }
        }
    }

}