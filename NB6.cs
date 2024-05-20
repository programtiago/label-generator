using QRCoder;
using System.Drawing.Drawing2D;
using Image = System.Drawing.Image;
using System.Drawing.Printing;
using Newtonsoft.Json;
using pocketlabeldata;

public class NB6{
    NB6 o = null;
    private Form1 frm1 = new Form1();
    public string sn { get; set; }
    public string mac { get; set; }
    public string networkWifiOne { get; set; }
    public string networkWifiTwo { get; set; }
    public string wifiPass { get; set; }

    private String transformQrCode(){
        return "Wifi Key:" + o.wifiPass;
    }

    public NB6(){}
    
    public NB6(String json){
        o = JsonConvert.DeserializeObject<NB6>(json);      
    }

    public String extractLastFourCharacters(String mac){
        String extractedString = mac.Substring(mac.Length - 4);
        return extractedString;
    }

    public void renderAndPrintWifi(object sender, PrintPageEventArgs e){

        Graphics g = e.Graphics;
        e.Graphics.SmoothingMode = SmoothingMode.None;
        e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality; 

        Font fontBold = new Font("Arial", 5.6f, FontStyle.Bold);
        Font fontItalic = new Font("Arial", 4.5f, FontStyle.Italic);

        SolidBrush brush = new SolidBrush(System.Drawing.Color.Black);
               
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(transformQrCode(), QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);
        Image qrCodeImage = qrCode.GetGraphic(20);

        var qrCodeImageWidth = qrCodeImage.Width;
        var qrCodeImageHeight = qrCodeImage.Height;  

        g.DrawString("Nom du réseau 2.4GHz:", fontItalic, brush, new System.Drawing.PointF(3, 26.5f));
        g.DrawString(o.networkWifiOne, fontBold, brush, new System.Drawing.PointF(3, 37));
        g.DrawString("Nom du réseau 5GHz:", fontItalic, brush, new System.Drawing.PointF(3, 50.5f));
        g.DrawString(o.networkWifiTwo, fontBold, brush, new System.Drawing.PointF(3, 61));
        g.DrawString("Clé de sécurité WiFi:", fontItalic, brush, new System.Drawing.PointF(3, 74.5f));
        g.DrawString(o.wifiPass, fontBold, brush, new System.Drawing.PointF(3, 85));

        g.DrawImage(qrCodeImage, 71.5f, 5.5f, (float)(qrCodeImageWidth*0.08), (float)(qrCodeImageHeight*0.08));
        
    }

    public void print(String printer, String model){

        PrintDocument printDocument = new PrintDocument();
        PrintDialog printDialog = new PrintDialog();

        try{                

            printDocument.DefaultPageSettings.PrinterSettings.PrinterName = printer;

            if (model == "NB6 WiFi"){
                printDocument.PrintPage += new PrintPageEventHandler(renderAndPrintWifi);
            }
        
            printDialog.Document = printDocument;
            printDocument.Print();
                
        }catch(InvalidPrinterException ex){
            DialogResult dr = MessageBox.Show("Impressora inválida. Por favor verifique se a impressora tem o nome NB6_WIFI !", "Erro impressora", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (dr == DialogResult.OK){
                frm1.serialBoxRefurbPage.Focus();
            }
        } 

    }
}