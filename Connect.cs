using GenCode128;
using QRCoder;
using System.Drawing.Drawing2D;
using Image = System.Drawing.Image;
using System.Drawing.Printing;

public class Connect{
    public string sn {get;set;}
    public string mac {get;set;}
    public string ean {get;set;}

    public Connect(String serial, String macAddress){
        this.sn = serial;
        this.mac = macAddress;
        this.ean = "3544168523331";
    }

    public String transformQrCode(){
        return "EAN: " + ean + ";S/N:" + sn;
    }

    public void renderAndPrint(object sender, PrintPageEventArgs e){
        Graphics g = e.Graphics;
        e.Graphics.SmoothingMode = SmoothingMode.None;
        e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality; 

        Image serialBarcode = Code128Rendering.MakeBarcodeImage(sn, 1, true);
        Image macBarcode   = Code128Rendering.MakeBarcodeImage(mac, 1, false);
        Image eanBarcode   = Code128Rendering.MakeBarcodeImage(ean, 1, false);

        float height = serialBarcode.Height;
        float width = serialBarcode.Width;

        Font fontBarcodes = new Font("Arial", 6);
        Font fontRegular = new Font("Arial", 5.9f);
        Font title = new Font("Arial", 5.3f, FontStyle.Bold);

        SolidBrush brush = new SolidBrush(System.Drawing.Color.Black);
               
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(transformQrCode(), QRCodeGenerator.ECCLevel.L);
        QRCode qrCode = new QRCode(qrCodeData);
        Image qrCodeImage = qrCode.GetGraphic(20);

        var qrCodeImageWidth = qrCodeImage.Width;
        var qrCodeImageHeight = qrCodeImage.Height;  
        g.DrawImage(qrCodeImage, 249.5f, 14.5f, (float)(qrCodeImageWidth*0.06), (float)(qrCodeImageHeight*0.06)); 

        var bottomImage = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\ce_emballage.png");
        //var bottomImage = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\ce_emballage.png");
        var bottomImageWidth = bottomImage.Width;
        var bottomImageHeight = bottomImage.Height;  
        g.DrawImage(bottomImage, 208.5f, 101.5f, (float)(bottomImageWidth*0.27), (float)(bottomImageHeight*0.25));


        var ceImage = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\ce.png");
        var ceImageWidth = ceImage.Width;
        var ceImageHeight = ceImage.Height;  
        //g.DrawImage(ceImage, 105f, 104.5f, (float)(ceImageWidth*0.15), (float)(ceImageHeight*0.15));
        
        var thrashImage = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\ce.png");
        //var thrashImage = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\Caixote_de_Lixo.png");
        var thrashImageWidth = thrashImage.Width;
        var thrashImageHeight = thrashImage.Height;   
        //g.DrawImage(thrashImage, 136.5f, 102.5f, (float)(thrashImageWidth*0.17), (float)(thrashImageHeight*0.17)); 

        var trimanImage = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\triman.png");
        //var trimanImage = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\triman.png");
        var trimanImageWidth = trimanImage.Width;
        var trimanImageHeight = trimanImage.Height;   
        //g.DrawImage(trimanImage, 217.5f, 102.5f, (float)(trimanImageWidth*0.40), (float)(trimanImageHeight*0.40)); 

        g.DrawString("CONNECT TV V2 NEW", title, brush, new System.Drawing.PointF(103.5f, 12.5f));
        g.DrawString("852333", new Font("Arial", 5.3f), brush, new System.Drawing.PointF(198.5f, 12.8f));

        var rating = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\triman.png");
        //var rating = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\rating.png");
        var widthImageRating = rating.Width;
        var heightImageRating = rating.Height;
        //g.DrawImage(rating, 148.5f, 0.5F, (float)(widthImageRating*0.06), (float)(heightImageRating*0.06));

        g.DrawImage(serialBarcode, 100.5f, 31.5f, (float)(width*0.50), (float)(height*0.45));
        g.DrawImage(macBarcode, 105.5f, 65.5f, (float)(width*0.46), (float)(height*0.45));
        g.DrawImage(eanBarcode, 105.5f, 98.8f, (float)(width*0.46), (float)(height*0.45));

        g.DrawString("S/N: " + sn, fontBarcodes, brush, new System.Drawing.PointF(103.5f, 49.2f));
        g.DrawString("MAC: " + mac, fontBarcodes, brush, new System.Drawing.PointF(103.5f, 84.5f));
        g.DrawString("EAN:" + ean, fontBarcodes, brush, new System.Drawing.PointF(103.5f, 116.8f));

        g.DrawString("Rating:12.0V == 1.0A", new Font("Arial", 4.3f), brush, new System.Drawing.PointF(230.5f, 57.5f));
        g.DrawString("Model:       DV8555", new Font("Arial", 4.3f), brush, new System.Drawing.PointF(230.5f, 67.5f));
        g.DrawString("Importer:   SFR", new Font("Arial", 4.3f), brush, new System.Drawing.PointF(230.5f, 77.5f));

        var sfrLogoImage = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\sfr_logo.png");
        //var sfrLogoImage = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\sfr_logo.png");
        var widthLogoImage = sfrLogoImage.Width;
        var heightLogoImage = sfrLogoImage.Height;
        g.DrawImage(sfrLogoImage, 228.5f, 19.3f, (float)(widthLogoImage*0.25), (float)(heightLogoImage*0.25));

        g.DrawString("Fabriqué en Chine", fontRegular, brush, new System.Drawing.PointF(104f, 138.5f));
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
            DialogResult dr = MessageBox.Show("Impressora inválida. Por favor verifique se a impressora tem o nome NB8_SN", "Impressora inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } 
    }

}