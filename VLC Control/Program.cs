using System;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static readonly HttpClient client = new HttpClient();

    const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
    const uint KEYEVENTF_EXTENDEDKEY = 1;
    const uint KEYEVENTF_KEYUP = 2;

    [DllImport("user32.dll")]
    public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

    static async Task Main(string[] args)
    {
        try
        {
            await ToggleVLCPlayPause();
            ToggleMediaPlayPause();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }

    // ... [Rest of your VLC control code] ...
    static async Task ToggleVLCPlayPause()
    {
        var vlcUrl = "http://localhost:8080/requests/status.xml?command=pl_pause";
        var byteArray = System.Text.Encoding.ASCII.GetBytes(":lolersauce"); // Replace with your VLC password
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        HttpResponseMessage response = await client.GetAsync(vlcUrl);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
    }


    static void ToggleMediaPlayPause()
    {
        keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
        keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
    }
}
