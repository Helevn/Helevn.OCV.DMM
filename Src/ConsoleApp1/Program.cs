// See https://aka.ms/new-console-template for more information
using Helevn.OCV.DMM;

while (true)
{
    var ctrl = new OcvCtrl("OCV1");
    try
    {
        await ctrl.EnsureConnectAsync();
        while (true)
        {
            var v = await ctrl.ReadAsync(DefaultCmd.ReadOcvCmd);
            Console.WriteLine($"值等于:{v}");
            await Task.Delay(1000);
        }
    }
    catch (Exception ex)
    {
        await ctrl.DisposeAsync();
        Console.WriteLine(ex.Message);
    }
    await Task.Delay(1000);
}
