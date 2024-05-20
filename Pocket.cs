using Newtonsoft.Json;
using GenCode128;
using QRCoder;
using System.Drawing.Drawing2D;
using Image = System.Drawing.Image;
using System.Drawing.Printing;
using pocketlabeldata;

public class Pocket{
    Form1 frm1 = new Form1();
    public string Imei { get; set; }
    public string Msn { get; set; }
    public string Ssid { get; set; }
    public string Wifipassword { get; set; }
    Pocket o = null;

    private String transformQrCode(){
        return o.Imei + ";" + o.Msn + ";" + "wifi ssid: " + o.Ssid + ";" + "wifi key: "+ o.Wifipassword;
    }

    public Pocket(){}

    public Pocket(String json){
        o = JsonConvert.DeserializeObject<Pocket>(json);
    }

    public void renderAndPrint(object sender, PrintPageEventArgs e){

            Graphics g = e.Graphics;
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            Image imei = Code128Rendering.MakeBarcodeImage(o.Imei, 1, false);
            Image serial   = Code128Rendering.MakeBarcodeImage(o.Msn, 1, false);
            
            float height = imei.Height;
            float width = imei.Width;

            Font fontBarcodes = new Font("Arial", 6.5f);
            Font fontRegular = new Font("Arial", 5, FontStyle.Bold);

            Font footerFontBold = new Font("Arial", 6.5f, FontStyle.Bold);
            Font title = new Font("Arial", 7, FontStyle.Bold);
            Font zteTitle = new Font("Arial", 7, FontStyle.Regular);
            Font modelTitle = new Font("Arial", 7, FontStyle.Regular);

            SolidBrush brush = new SolidBrush(System.Drawing.Color.Black);
        
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //L - 7%  M - 15%  Q - 25%  H - 30%   densidade do qr code  consultei https://kazuhikoarase.github.io/qrcode-generator/js/demo/
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(transformQrCode(), QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            Image qrCodeImage = qrCode.GetGraphic(20);

            var qrCodeImageWidth = qrCodeImage.Width;
            var qrCodeImageHeight = qrCodeImage.Height;
            g.DrawImage(qrCodeImage, 201.5f, 81.5f, (float)(qrCodeImageWidth*0.07), (float)(qrCodeImageHeight*0.07));    

            g.DrawString("BOX DE POCHE 4G", title, brush, new System.Drawing.PointF(15, 10));
            g.DrawString("ZTE", zteTitle, brush, new System.Drawing.PointF(238,12));
            g.DrawString("MODEL:MF920U", modelTitle, brush, new System.Drawing.PointF(183.5f, 27.5f));


            //var imageCE = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\CE.png");
            var imageCE = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\CE.png");
            var widthImageCe = imageCE.Width;
            var heightImageCe = imageCE.Height;
            //var imageTrash = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\Caixote_de_Lixo.png");
            var imageTrash = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\Caixote_de_Lixo.png");
            var widthImageTrash = imageTrash.Width;
            var heightImageTrash = imageTrash.Height;

            g.DrawImage(imageCE, 197.5f, 37, (float)(widthImageCe*0.15), (float)(heightImageCe*0.15));
            g.DrawImage(imageTrash, 235.5f, 42.5f, (float)(widthImageTrash*0.15), (float)(heightImageTrash*0.15));

            g.DrawImage(imei, 17.5f, 27.5f, (float)(width*0.68), (float)(height*0.45));
            g.DrawImage(serial, 17.5f, 55.5f, (float)(width*0.55), (float)(height*0.45));

            g.DrawString("IMEI: " + o.Imei, fontBarcodes, brush, new System.Drawing.PointF(17, 42));
            g.DrawString("S/N: " + o.Msn, fontBarcodes, brush, new System.Drawing.PointF(17, 69.5f));

            g.DrawString("WiFi SSID: " + o.Ssid, fontRegular, brush, new System.Drawing.PointF(17, 83.5f));
            g.DrawString("WiFi Key: " + o.Wifipassword, fontRegular, brush, new System.Drawing.PointF(17, 93.5f));
            g.DrawString("Device Manager Website: http://192.168.0.1", fontRegular, brush, new System.Drawing.PointF(17, 103.5f));
            g.DrawString("Website Password: admin", fontRegular, brush, new System.Drawing.PointF(17, 113.5f));
            g.DrawString("Made in China", footerFontBold, brush, new System.Drawing.PointF(17, 124.5f));
    }

    public void print(String printer){

        PrintDocument printDocument = new PrintDocument();
        PrintDialog printDialog = new PrintDialog();

        try{                    
            printDocument.DefaultPageSettings.PrinterSettings.PrinterName = printer;

            printDocument.PrintPage += new PrintPageEventHandler(renderAndPrint);

            printDialog.Document = printDocument;
            printDocument.Print();  
                
        }catch(InvalidPrinterException ex){
            DialogResult dr = MessageBox.Show("Impressora inválida. Por favor verifique se a impressora tem o nome POCKET_SN !", "Erro impressora", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (dr == DialogResult.OK){
                frm1.serialBoxRefurbPage.Focus();
            }
        } 

    }

}