using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using GenCode128;
using Newtonsoft.Json;
using QRCoder;
using Excel = Microsoft.Office.Interop.Excel;


namespace pocketlabeldata;

public partial class Form1 : Form
{
    int numberPallets = 0;
    int numberPalletsFormated;
    public string responseBody;
    private System.Data.DataTable dataTable; 
    private Connect connect;
    GroupBoxLabel groupBox;
    public List<GroupBoxLabel> dataLabel = [];
    private int currentPageIndex = 0;
    string cellValue = "";
    public string macAddress = "";
    public string sn = "";
    public string snGpon = "";        
    public string wifiName = "";
    public string wifiPassword = "";
    public string modelEquipment = "";
    private string modelName = "";
    private ContextMenuStrip contextMenu = new ContextMenuStrip();
    
    public Form1()
    {
        InitializeComponent();

        initializePrintersListComboBox();
        InitializeContextMenu();
        InitializeDataTable();

        //refurbGroupingLabelTabPage.Enabled = false;

        notAllowResizingMaxAndMin();
        
        closeButtonForm.TabStop = false;

        initializeModelListComboForIdPallet();
        initializeModelListCombo();
    
        modelComboBox.Select();

        insertQuantityManually.Checked = true;
        
        printButton.Enabled = true;
        printAutomatic.Checked = true;    
    }

    private void initializePrintersListComboBox(){
        foreach(string printerName in PrinterSettings.InstalledPrinters){
            printersListComboBox.Items.Add(printerName);
        }
    }

    private void InitializeContextMenu(){
        ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem
        {
            Text = "Imprimir"
        };

        toolStripItem1.Click += new EventHandler(onContextMenuClick);

        contextMenu.Items.Add(toolStripItem1);

        ContextMenuStrip = contextMenu;
    }

    private void InitializeDataTable()
    {
        dataTable = new System.Data.DataTable();
        dataTable.Columns.Add("ID Palete", typeof(string));
        dataTable.Columns.Add("Group Box", typeof(int));
        dataTable.Columns.Add("Artigo", typeof(int));
        dataTable.Columns.Add("Quantidade", typeof(int));
    }

    private void generateLabelDataConnect(){
        connect = new Connect(serialBoxRefurbPage.Text, macBoxRefurbPage.Text);
    }
    
    private bool checkAutoPrint(){
        if (printAutomatic.Checked == true){
            return true;
        }

        return false;
    }
    
    private void initializeModelListCombo(){
       modelComboBox.Items.Add("Box de Poche 4G");
       modelComboBox.Items.Add("NB8 2P");
       modelComboBox.Items.Add("NB8 4P");
       modelComboBox.Items.Add("NB6 WiFi");
       modelComboBox.Items.Add("CONNECT TV V2 NEW");

    }

     private void initializeModelListComboForIdPallet(){
       idPalletModelComboBox.Items.Add("Box de Poche 4G");
       idPalletModelComboBox.Items.Add("Box de Poche 4G MEIG");
       idPalletModelComboBox.Items.Add("Repeteaur AX1800");
    }

    private String transformQrCode2P(){
            return "3544168521306" + ";" + sn;
    }

    private String transformQrCode4P(){
        return "3544168521306" + ";" + sn + ";" + macAddress + ";" + snGpon;
    }
    private bool validateModel(){
        return modelComboBox.SelectedIndex > -1;
    }

    private bool getUnitData(){
        //String jsonWiFiNB6 = @"C:\Users\User\IdeaProjects\pocketlabeldata\NB6.json";
        String jsonWiFiNB6 = @"C:\Program Files\Label Generator\NB6.json";
        
        var url = "https://refurb-as:8444/mp/ws/testunit/preload/?partno=MF920U&key=imei&value=" + serialBoxRefurbPage.Text;
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback += (message, cert, chain, sslPolicyErrors) =>
        {
            return true;
        };

        try
        {
            HttpClient httpClient = new HttpClient();
            httpClient = new HttpClient(httpClientHandler){
            BaseAddress = new Uri(url)
        };

        var response = httpClient.Send(request);
        using var reader = new StreamReader(response.Content.ReadAsStream());
        responseBody = reader.ReadToEnd();

        if (responseBody != null)
        {
            if (modelComboBox.Text == "Box de Poche 4G"){
                dataResponse.Text = responseBody;
                return true;
            }else if(modelComboBox.Text == "NB6 WiFi"){
                using (StreamReader reader4 = new StreamReader(jsonWiFiNB6)){
                    string jsonFile = reader.ReadToEnd();
                    object jsonResponse = JsonConvert.DeserializeObject(File.ReadAllText(jsonWiFiNB6));
                    dataResponse.Text = jsonResponse.ToString();       
                    return true;         
                }
            }else if(modelComboBox.Text == "CONNECT TV V2 NEW"){
                return true;
            }else if(modelComboBox.Text == "NB8 4P" || modelComboBox.Text == "NB8 2P"){
        
                    Excel.Application appExcel = new Excel.Application();
                    //Excel.Workbook xlWorkbook = appExcel.Workbooks.Open(@"C:\Users\User\IdeaProjects\pocketlabeldata\nb8_label_info.xlsx");
                    Excel.Workbook xlWorkbook = appExcel.Workbooks.Open(@"C:\Program Files\Label Generator\nb8_label_info.xlsx");
                    Excel.Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                    Excel.Range xlRange = xlWorksheet.UsedRange;

                    string searchValue = serialBoxRefurbPage.Text;

                    Excel.Range foundCell = xlRange.Find(searchValue, Type.Missing, Type.Missing, Excel.XlLookAt.xlWhole,
                        Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, false, Type.Missing, Type.Missing);

                if (foundCell != null){
                    int row = foundCell.Row;
                    Excel.Range rowRange = xlWorksheet.Rows[row];

                    for (int i = 1; i <= xlRange.Columns.Count; i++){
                        string cellValue = Convert.ToString(rowRange.Cells[1, i].Value);


                        snGpon = Convert.ToString(rowRange.Cells[1, 7].Value);
                        macAddress = Convert.ToString(rowRange.Cells[1, 1].Value);
                        modelName = Convert.ToString(rowRange.Cells[1, 2].Value);
                        Console.WriteLine("Nome do modelo: " + modelName);
                        sn = Convert.ToString(rowRange.Cells[1,5].Value);
                        wifiName = Convert.ToString(rowRange.Cells[1,3].Value);
                        wifiPassword = Convert.ToString(rowRange.Cells[1,4].Value);

                        modelEquipment = Convert.ToString(rowRange.Cells[1,6].Value);

                        if (modelEquipment.StartsWith("R0") && modelComboBox.Text == "NB8 4P"){
                            modelEquipment = "ALGP2-ALB-r0";
                        }else if (modelEquipment.StartsWith("R1") && modelComboBox.Text == "NB8 4P"){
                            modelEquipment = "ALGP2-ALB-r1";
                        }else if (modelEquipment.StartsWith("R2") && modelComboBox.Text == "NB8 4P"){
                            modelEquipment = "ALGP2-ALB-r2";
                        }else if (modelEquipment.StartsWith("R3") && modelComboBox.Text == "NB8 4P"){
                            modelEquipment = "ALGP2-ALB-r3";
                        }else if (modelEquipment.StartsWith("R4") && modelComboBox.Text == "NB8 4P"){
                            modelEquipment = "ALGP2-ALB-r4";
                        }else if (modelEquipment.StartsWith("R0") && modelComboBox.Text == "NB8 2P"){
                            modelEquipment = "ALGP1-ALB-r0";
                        }else if (modelEquipment.StartsWith("R1") && modelComboBox.Text == "NB8 2P"){
                            modelEquipment = "ALGP1-ALB-r1";
                        }else if (modelEquipment.StartsWith("R2") && modelComboBox.Text == "NB8 2P"){
                            modelEquipment = "ALGP1-ALB-r2";
                        }else if (modelEquipment.StartsWith("R3") && modelComboBox.Text == "NB8 2P"){
                            modelEquipment = "ALGP1-ALB-r3"; 
                        }else if (modelEquipment.StartsWith("R4") && modelComboBox.Text == "NB8 2P"){
                            modelEquipment = "ALGP1-ALB-r4";     
                        }         

                        if (modelComboBox.Text == "NB8 2P" && modelName != "GR120DG"){
                            focusSerialBoxAndSelect();
                            MessageBox.Show("O modelo que introduziu não corresponde ao modelo do número de série introduzido", "Modelo inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }else if (modelComboBox.Text == "NB8 4P" && modelName != "GR140DG"){
                            focusSerialBoxAndSelect();
                            MessageBox.Show("O modelo que introduziu não corresponde ao modelo do número de série introduzido", "Modelo inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        dataResponse.Text = snGpon + "\n\t" + macAddress + "\n\t" + sn + "\n\t" + wifiName + "\n\t" + wifiPassword;
                        return true;
                    }
                }else if (foundCell == null){
                    focusSerialBoxAndSelect();
                    dataResponse.Text = "{}";
                }   
                    
                    // Cleanup
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                    appExcel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(appExcel);
            }
        }
            }catch(Exception e){
            MessageBox.Show("Impossível carregar os recursos solicitados");
  
        }
        return false;
    }

     private String splitMac(String mac){

         var exp = Regex.Replace(mac, ".{2}", "$0:");
         var delimitter = exp.Trim(':');
         mac = delimitter;

        return mac;
    }

    private bool validateSerialNumberNB8(){
        if (!serialBoxRefurbPage.Text.StartsWith("LP")){
            focusSerialBoxAndSelect();
            MessageBox.Show("O serial number deve começar por LP");
            return false;
        }
        return true;
    }
        
    private void callWebService_OnFocus(object sender, EventArgs e){    
                if (checkAutoPrint() == true && validateSerialNumberNB8() == true){
                    do_print();
                }
                focusSerialBoxAndSelect();
    }

    private void printButton_OnFocus(object sender, EventArgs e){ 
        do_print();
    }

    public void notAllowResizingMaxAndMin(){
        FormBorderStyle = FormBorderStyle.None;
        MaximizeBox = false;
        MinimizeBox = false;   
    }

    private void formClose_click(object sender, EventArgs e){
            DialogResult dr = MessageBox.Show("Tem a certeza que pretende sair ? ", "Tem a certeza ? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes){
                    Close();
                }else{
                    focusSerialBoxAndSelect();
                }      
    }

    private void focusSerialBoxAndSelect(){
        serialBoxRefurbPage.Focus();
        serialBoxRefurbPage.SelectAll();
    }
   
    private void do_print(){

        string modelChoosen = modelComboBox.Text;

        if (!validateModel()){
            modelComboBox.Focus();
            DialogResult dr = MessageBox.Show("É obrigatório selecionar um modelo ! ", "Modelo obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }else{
                if (!getUnitData()){
                        focusSerialBoxAndSelect();
                        MessageBox.Show("Número de série não foi encontrado");
            }           
        }

            if (dataResponse.Text != "{}"){
                    switch(modelChoosen){
                    case "Box de Poche 4G":
                        new Pocket(dataResponse.Text).print("POCKET_SN");
                        break;
                    case "NB8 2P":
                        if (modelName != "GR120DG"){
                            focusSerialBoxAndSelect();
                            MessageBox.Show("Impossível imprimir. Alguma coisa correu mal...", "Erro na impressão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }else{
                            print("NB8_SN", modelChoosen);
                            break;
                        }
                        break;
                        
                    case "NB8 4P":
                        if (modelName != "GR140DG"){
                            focusSerialBoxAndSelect();
                            MessageBox.Show("Impossível imprimir. Alguma coisa correu mal...", "Erro na impressão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }else{
                            print("NB8_SN", modelChoosen);
                            break;
                        } 
                            break; ;
                    case "NB6 WiFi":
                        new NB6(dataResponse.Text).print("NB6_WIFI", modelChoosen);
                        break; 
                    case "CONNECT TV V2 NEW":
                        generateLabelDataConnect();
                        new Connect(connect.sn, connect.mac).print("CONNECT_SN");
                        break;    
                    case "":
                        break;                 
                    default:
                        MessageBox.Show("Modelo ainda não implementado.");
                        break;
                    }
            }else{
                focusSerialBoxAndSelect();
        }
    }

    private void PrintButton_click(object sender, EventArgs e){ 
        do_print();
    }
    public void numberPalletsBox_Leave(object sender, EventArgs e)  {        
        if (numberPalletsBox.Text == ""){
            
            MessageBox.Show("Obrigatório preencher o número de paletes", "Número de paletes em falta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            numberPalletsBox.Focus();
            numberPalletsBox.SelectAll();
        }else if (System.Text.RegularExpressions.Regex.IsMatch(numberPalletsBox.Text, "[^0-9]")){
            numberPalletsBox.Focus();
            numberPalletsBox.SelectAll();
            MessageBox.Show("Por favor, insira só números ! ", "Validação número de caixas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }else{
            Int32.TryParse(idPalletBox.Text, out numberPallets);
        }
    }

    private bool checkIfQuantityIsOk(){
        if (idPalletModelComboBox.Text == "Box de Poche 4G"){
            if (Int32.Parse(numberPalletsBox.Text) > 16) return false;
        }else if(idPalletModelComboBox.Text == "Repeteaur AX1800"){
            if (Int32.Parse(numberPalletsBox.Text) > 24) return false;
        }else if(idPalletModelComboBox.Text == "Box de Poche 4G MEIG")
            if (Int32.Parse(numberPalletsBox.Text) > 16) return false;      

        return true;
    }

    private bool checkForDuplicatesDataLabel(){
         for (int i = 0; i < dataLabel.Count; i++){
            if (dataLabel[i].groupBoxId == idPalletBox.Text){
                idPalletBox.Focus();
                idPalletBox.SelectAll();
                MessageBox.Show("Esse ID Palete já foi adicionado anteriormente ! ", "ID Palete Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        return false;
    }

    public void insertIdPalletValues_OnFocus(object sender, EventArgs e){
        groupBox = new GroupBoxLabel();
        if (idPalletModelComboBox.SelectedIndex != -1){
            if (idPalletBox.Text.StartsWith("P-") && idPalletBox.TextLength == 13){
                if (numberPallets < Int32.Parse(numberPalletsBox.Text)){
                    if (idPalletModelComboBox.Text == "Box de Poche 4G"){
                        if (!checkForDuplicatesDataLabel()){
                            if (checkIfQuantityIsOk()){
                                groupBox.codeArticle = 852495;
                                groupBox.quantity = 82;
                                groupBox.groupBoxId = idPalletBox.Text;
                                groupBox.numberPallets = Int32.Parse(numberPalletsBox.Text);
                                numberPallets++;
                            }else{
                                numberPalletsBox.Focus();
                                numberPalletsBox.SelectAll();
                                MessageBox.Show("A quantidade para este modelo não permite mais de 16 caixas", "Erro número caixas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                            }else if(idPalletModelComboBox.Text == "Repeteaur AX1800"){
                                if (!checkForDuplicatesDataLabel()){
                                    if (checkIfQuantityIsOk()){
                                        groupBox.codeArticle = 852501;
                                        groupBox.quantity = 11;
                                        groupBox.groupBoxId = idPalletBox.Text;
                                        groupBox.numberPallets = Int32.Parse(numberPalletsBox.Text);
                                        numberPallets++;
                                    }else{
                                        numberPalletsBox.Focus();
                                        numberPalletsBox.SelectAll();
                                        MessageBox.Show("A quantidade para este modelo não permite mais de 24 caixas", "Erro número caixas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                
                            }else if (idPalletModelComboBox.Text == "Box de Poche 4G MEIG"){
                                if (!checkForDuplicatesDataLabel()){
                                    if (checkIfQuantityIsOk()){
                                        groupBox.codeArticle = 852573;
                                        groupBox.quantity = 82;
                                        groupBox.groupBoxId = idPalletBox.Text;
                                        groupBox.numberPallets = Int32.Parse(numberPalletsBox.Text);
                                        numberPallets++;
                                    }                        
                                }
                        }else{
                            MessageBox.Show("É obrigatório selecionar o modelo ! ", "Modelo obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    
                        if (!checkForDuplicatesDataLabel()){
                            //InitializeDataTable();
                            DataRow newRow = dataTable.NewRow();

                            newRow["ID Palete"] = idPalletBox.Text;
                            newRow["Group Box"] = numberPallets;
                            newRow["Quantidade"] = groupBox.quantity;
                            newRow["Artigo"] = groupBox.codeArticle;

                            dataTable.Rows.Add(newRow);
                            dataGridViewPallet.DataSource = dataTable;

                            int newRowIndex = dataGridViewPallet.Rows.Count - 1;
    
                            dataGridViewPallet.Rows[newRowIndex].Cells[0].Value = groupBox.groupBoxId;
                            dataGridViewPallet.Rows[newRowIndex].Cells[1].Value = numberPallets;
                            dataGridViewPallet.Rows[newRowIndex].Cells[2].Value = groupBox.codeArticle;
                            dataGridViewPallet.Rows[newRowIndex].Cells[3].Value = groupBox.quantity;

                            dataGridViewPallet.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewPallet.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewPallet.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewPallet.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; 

                            dataLabel.Add(groupBox);    

                            idPalletBox.Focus();
                            idPalletBox.SelectAll();

                            counterNumberPallets.Visible = true;
                            counterNumberPallets.Text = numberPallets + "/" + numberPalletsBox.Text;
                        }                                     
                }else if (numberPallets == Int32.Parse(numberPalletsBox.Text)){
                    idPalletBox.Focus();
                    MessageBox.Show("Não é possível introduzir mais paletes. As " + numberPallets + " já foram lidas com sucesso !", "Contador atingido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                }
                
                }else{
                    idPalletBox.Focus();
                    idPalletBox.SelectAll();
                } 
        }else{
            idPalletModelComboBox.Focus();
            MessageBox.Show("É obrigatório selecionar o modelo ! ", "Modelo obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private bool checkQuantityInsertCheckBox(){
        bool isChecked;

        if (insertQuantityManually.Checked){
            numberPalletsBox.Enabled = false;
            isChecked = true;
        }else{
            numberPalletsBox.Enabled = true;
            isChecked = false;
        }

        return isChecked;
    }

    public void idPalletModelComboBox_SelectedIndexChanged(object sender, EventArgs e){
        if (checkQuantityInsertCheckBox()){
            if (idPalletModelComboBox.Text == "Box de Poche 4G"){
                numberPalletsBox.Text = "16";
            }else if(idPalletModelComboBox.Text == "Box de Poche 4G MEIG"){
                numberPalletsBox.Text = "16";
            }else if(idPalletModelComboBox.Text == "Repeteaur AX1800"){
                numberPalletsBox.Text = "24";
            }
        }
    }

     public void printButtonLabelIdPallet_click(object sender, EventArgs e){
      try{ 
        if (numberPallets == Int32.Parse(numberPalletsBox.Text) && printersListComboBox.Text != ""){
            Print();
            idPalletModelComboBox.Text = "";
            numberPalletsBox.Text = "";
            idPalletBox.Text = "";
            numberPallets = 0;
            modelComboBox.Focus();
            counterNumberPallets.Visible = false;
        }else if (modelComboBox.SelectedIndex < -1){
            idPalletModelComboBox.Focus();
            MessageBox.Show("É obrigatório selecionar o modelo ! ", "Modelo obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }else if (numberPallets < Int32.Parse(numberPalletsBox.Text)){
            idPalletBox.Focus();
            idPalletBox.SelectAll();
            MessageBox.Show("Impossível efetuar a impressão. Contador de paletes não foi atingido ! ", "Erro contador paletes", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }else if (printersListComboBox.Text == ""){
            MessageBox.Show("Não selecionou uma impressora de destino !", "Impressora em falta", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }else if (numberPalletsBox.Text == ""){
            MessageBox.Show("Tem de preencher o número de caixas a imprimir", "Erro caixas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }catch(FormatException){
        MessageBox.Show("O formato de número de caixas é inválido ! ", "Formatação valor caixas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
     
    private void onContextMenuClick(object sender, EventArgs e){
        PrintRecorFromDataGrid();
            
    }

    public void print(String printer, String model){

        PrintDocument printDocument = new PrintDocument();
        PrintDialog printDialog = new PrintDialog();

        try{                

            printDocument.DefaultPageSettings.PrinterSettings.PrinterName = printer;

            if (model == "NB8 2P"){
                printDocument.PrintPage += new PrintPageEventHandler(renderAndPrint2P);
            }else if (model == "NB8 4P"){
                printDocument.PrintPage += new PrintPageEventHandler(renderAndPrint4P);
            }

            printDialog.Document = printDocument;
            printDocument.Print();
                
        }catch(InvalidPrinterException ex){
            DialogResult dr = MessageBox.Show("Impressora inválida. Por favor verifique se a impressora tem o nome NB8_SN", "Impressora inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (dr == DialogResult.OK){
                serialBoxRefurbPage.Focus();
            }
        } 
    }

    public void renderAndPrint2P(object sender, PrintPageEventArgs e){
        Graphics g = e.Graphics;
        e.Graphics.SmoothingMode = SmoothingMode.None;
        e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality; 

        System.Drawing.Image serial = Code128Rendering.MakeBarcodeImage(sn, 1, true);
        System.Drawing.Image serialGpon   = Code128Rendering.MakeBarcodeImage(snGpon, 1, false);
        macAddress = macAddress.Replace(":", "");
        System.Drawing.Image mac   = Code128Rendering.MakeBarcodeImage(macAddress, 1, false);

        float height = serial.Height;
        float width = serial.Width;

        System.Drawing.Font fontBarcodes = new System.Drawing.Font("Arial", 5);
        System.Drawing.Font fontRegular = new System.Drawing.Font("Arial", 5);
        System.Drawing.Font fontItalic = new System.Drawing.Font("Arial", 5.5f, FontStyle.Italic);
        System.Drawing.Font footerFontBold = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
        System.Drawing.Font title = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
        System.Drawing.Font nb8TitleModel = new System.Drawing.Font("Arial", 10, FontStyle.Regular);
        System.Drawing.Font client = new System.Drawing.Font("Arial", 6.5f, FontStyle.Bold);

        SolidBrush brush = new SolidBrush(System.Drawing.Color.Black);
               
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(transformQrCode2P(), QRCodeGenerator.ECCLevel.L);
        QRCode qrCode = new QRCode(qrCodeData);
        System.Drawing.Image qrCodeImage = qrCode.GetGraphic(20);

        var qrCodeImageWidth = qrCodeImage.Width;
        var qrCodeImageHeight = qrCodeImage.Height;   
        g.DrawImage(qrCodeImage, 196.5f, 6.5f, (float)(qrCodeImageWidth*0.08), (float)(qrCodeImageHeight*0.08)); 
        g.DrawString("SFR", client, brush, new System.Drawing.PointF(210.5f, 58));

        g.DrawString("MOD FIX NB8 ALTLAB FTTH", title, brush, new System.Drawing.PointF(48.5f, 2.5f));
        g.DrawString(modelEquipment, nb8TitleModel, brush, new System.Drawing.PointF(48.5f,12.5f));
        g.DrawString("ALTICE LABS", fontRegular, brush, new System.Drawing.PointF(199.5f, 72));
        g.DrawString("Model:GR120DG", fontRegular, brush, new System.Drawing.PointF(194.5f, 79));

        var voltageAndAmparage = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\voltage_amparage.png");
        //var voltageAndAmparage = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\voltage_amparage.png");
        var widthImagevoltageAndAmparage = voltageAndAmparage.Width;
        var heightImagevoltageAndAmparage = voltageAndAmparage.Height;

        //g.DrawImage(mac, 7.5f, 28.5f, (float)(width*0.55), (float)(height*0.45));  <- Valor original
        g.DrawImage(mac, 51.5f, 28.5f, (float)(width*0.46), (float)(height*0.40));
        g.DrawImage(serial, 46.5f, 53, (float)(width*0.50), (float)(height*0.40));
        g.DrawImage(serialGpon, 51.5f, 78, (float)(width*0.46), (float)(height*0.40));

        g.DrawString("MAC: " + splitMac(macAddress), fontBarcodes, brush, new System.Drawing.PointF(51.5f, 44));
        g.DrawString("S/N: " + sn, fontBarcodes, brush, new System.Drawing.PointF(51.5f, 69));
        g.DrawString("S/N GPON: " + snGpon, fontBarcodes, brush, new System.Drawing.PointF(51.5f, 93.5f));

        g.DrawString("Nom du réseau WiFi:", fontItalic, brush, new System.Drawing.PointF(252.5f, 10));
        g.DrawString(wifiName, footerFontBold, brush, new System.Drawing.PointF(252.5f,23.5F));
        g.DrawString("Clé de sécurité WiFi:", fontItalic, brush, new System.Drawing.PointF(252.5f, 37));
        g.DrawString(wifiPassword, footerFontBold, brush, new System.Drawing.PointF(252.5f, 50));

        g.DrawImage(voltageAndAmparage, 263.5f, 65.5f, (float)(widthImagevoltageAndAmparage*0.2), (float)(heightImagevoltageAndAmparage*0.2));
        
    }

    public void renderAndPrint4P(object sender, PrintPageEventArgs e){

        Graphics g = e.Graphics;
        e.Graphics.SmoothingMode = SmoothingMode.None;
        e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality; 

        System.Drawing.Image serial = Code128Rendering.MakeBarcodeImage(sn, 1, true);
        System.Drawing.Image serialGpon   = Code128Rendering.MakeBarcodeImage(snGpon, 1, false);
        macAddress = macAddress.Replace(":", "");
        System.Drawing.Image mac   = Code128Rendering.MakeBarcodeImage(macAddress, 1, false);

        float height = serial.Height;
        float width = serial.Width;

        System.Drawing.Font fontBarcodes = new System.Drawing.Font("Arial", 5);
        System.Drawing.Font fontRegular = new System.Drawing.Font("Arial", 5);
        System.Drawing.Font fontItalic = new System.Drawing.Font("Arial", 5.5f, FontStyle.Italic);
        System.Drawing.Font footerFontBold = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
        System.Drawing.Font title = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
        System.Drawing.Font nb8TitleModel = new System.Drawing.Font("Arial", 10, FontStyle.Regular);
        System.Drawing.Font client = new System.Drawing.Font("Arial", 6.5f, FontStyle.Bold);

        SolidBrush brush = new SolidBrush(System.Drawing.Color.Black);
               
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(transformQrCode4P(), QRCodeGenerator.ECCLevel.L);
        QRCode qrCode = new QRCode(qrCodeData);
        System.Drawing.Image qrCodeImage = qrCode.GetGraphic(20);

        var qrCodeImageWidth = qrCodeImage.Width;
        var qrCodeImageHeight = qrCodeImage.Height;   
        g.DrawImage(qrCodeImage, 185.5f, 11.5f, (float)(qrCodeImageWidth*0.06), (float)(qrCodeImageHeight*0.06)); 
        g.DrawString("SFR", client, brush, new System.Drawing.PointF(199.5f, 58));

        g.DrawString("MOD FIX NB8 4P ALTLAB FTTH MIXNW", title, brush, new System.Drawing.PointF(48.5f, 2.5f));
        g.DrawString(modelEquipment, nb8TitleModel, brush, new System.Drawing.PointF(48.5f,12.5f));
        g.DrawString("ALTICE LABS", fontRegular, brush, new System.Drawing.PointF(187.5f, 72));
        g.DrawString("Model:GR140DG", fontRegular, brush, new System.Drawing.PointF(182.5f, 79));

        var voltageAndAmparage = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\voltage_amparage.png");
        //var voltageAndAmparage = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\voltage_amparage.png");
        var widthImagevoltageAndAmparage = voltageAndAmparage.Width;
        var heightImagevoltageAndAmparage = voltageAndAmparage.Height;

        //g.DrawImage(mac, 7.5f, 28.5f, (float)(width*0.55), (float)(height*0.45));  <- Valor original
        g.DrawImage(mac, 51.5f, 28.5f, (float)(width*0.46), (float)(height*0.40));
        g.DrawImage(serial, 47f, 53, (float)(width*0.48), (float)(height*0.40));
        g.DrawImage(serialGpon, 51.5f, 78, (float)(width*0.46), (float)(height*0.40));

        g.DrawString("MAC: " + splitMac(macAddress), fontBarcodes, brush, new System.Drawing.PointF(51.5f, 44));
        g.DrawString("S/N: " + sn, fontBarcodes, brush, new System.Drawing.PointF(51.5f, 69));
        g.DrawString("S/N GPON: " + snGpon, fontBarcodes, brush, new System.Drawing.PointF(51.5f, 93.5f));

        g.DrawString("Nom du réseau WiFi:", fontItalic, brush, new System.Drawing.PointF(252.5f, 10));
        g.DrawString(wifiName, footerFontBold, brush, new System.Drawing.PointF(252.5f,23.5F));
        g.DrawString("Clé de sécurité WiFi:", fontItalic, brush, new System.Drawing.PointF(252.5f, 37));
        g.DrawString(wifiPassword, footerFontBold, brush, new System.Drawing.PointF(252.5f, 50));

        g.DrawImage(voltageAndAmparage, 263.5f, 65.5f, (float)(widthImagevoltageAndAmparage*0.2), (float)(heightImagevoltageAndAmparage*0.2));
        
    }
    public void renderAndPrintRecordEdited(object sender, PrintPageEventArgs e){
         
        System.Drawing.Rectangle headerTable = new System.Drawing.Rectangle

        {
            //Size = new System.Drawing.Size(295, 10),
            Size = new System.Drawing.Size(365, 10),
            Location = new System.Drawing.Point(20, 80),
        };

            Graphics g = e.Graphics;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            var imageLogo = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\Logo_Netceed.png");
            //var imageLogo = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\Logo_Netceed.png");

            var widthImageLogo = imageLogo.Width;
            var heightImageLogo = imageLogo.Height;

            SolidBrush brushTableColor = new SolidBrush(System.Drawing.Color.LightGray);
            g.FillRectangle(brushTableColor, headerTable);

            System.Drawing.Font bodyFont = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
            System.Drawing.Font titleFont = new System.Drawing.Font("Arial", 6.5f);
            //System.Drawing.Font titleFont = new System.Drawing.Font("Arial", 6.5f, FontStyle.Bold);

            Pen penRectangle = new Pen(System.Drawing.Color.Black, 2);
            SolidBrush brush = new SolidBrush(System.Drawing.Color.Black);

            int x = 35;
            int y = 80;

            String ean = "";

            int totalPallets = 0;

            if (dataGridViewPallet.SelectedRows[0].Cells[2].Value.ToString() == "852495"){
                totalPallets = 16;
                ean = "6902176038501";
            }else if (dataGridViewPallet.SelectedRows[0].Cells[2].Value.ToString() == "852573"){
                totalPallets = 16;
                ean = "6974889940044";
            }else if(dataGridViewPallet.SelectedRows[0].Cells[2].Value.ToString() == "852501"){
                totalPallets = 24;
                ean = "6971571800204";
            }

            System.Drawing.Image barcode1 = Code128Rendering.MakeBarcodeImage("6902176038501", 1, false);
            
            float heightBarcode1 = barcode1.Height;
            float widthBarcode1  = barcode1.Width;   

            g.DrawString("Group Box ID: GB-" + dataGridViewPallet.SelectedRows[0].Cells[0].Value.ToString(), bodyFont, brush, new System.Drawing.PointF(20, y - 18));
        
 
            g.DrawString("Group Box: " + dataGridViewPallet.SelectedRows[0].Cells[1].Value + "/" + totalPallets , bodyFont, brush, new System.Drawing.PointF(286, y - 18));


            g.DrawString(dataGridViewPallet.SelectedRows[0].Cells[3].Value.ToString(), bodyFont, brush, new System.Drawing.PointF(x, 108));
            g.DrawString(dataGridViewPallet.SelectedRows[0].Cells[2].Value.ToString(), bodyFont, brush, new System.Drawing.PointF(x + 52, 108.7f));
          
            g.DrawImage(imageLogo, 10, 15, (float)(widthImageLogo*0.10), (float)(heightImageLogo*0.10));
            g.DrawRectangle(penRectangle , headerTable);
            g.DrawString("Qt", titleFont, brush, new System.Drawing.PointF(x, y));
            g.DrawLine(penRectangle, 65, 80, 65, 90); 
            g.DrawString("Ref.Client", titleFont, brush, new System.Drawing.PointF(x + 52, y));
            g.DrawLine(penRectangle, 155, 80, 155, 90);  
            g.DrawString("Description", titleFont, brush, new System.Drawing.PointF(242, y));

            y += 30;

            g.DrawImage(barcode1, 197, y - 12, widthBarcode1 * 0.85f, heightBarcode1 * 0.85f);
            g.DrawString(ean, new System.Drawing.Font(new System.Drawing.Font("Arial", 5.5f), FontStyle.Regular), brush, new System.Drawing.PointF(239.5f, 122));
        
            if (dataGridViewPallet.SelectedRows[0].Cells[2].Value.ToString() == "852495"){
                g.DrawString("BOX DE POCHE 4G LEGO MIXRC", new System.Drawing.Font(new System.Drawing.Font("Arial", 7), FontStyle.Regular), brush, new System.Drawing.PointF(194.5f, 135));
            }else if (dataGridViewPallet.SelectedRows[0].Cells[2].Value.ToString() == "852501"){
                g.DrawString("REPETEUR AX1800 SDMC MIXRC", new System.Drawing.Font(new System.Drawing.Font("Arial", 7), FontStyle.Regular), brush, new System.Drawing.PointF(194.5f, 135));
            }else if (dataGridViewPallet.SelectedRows[0].Cells[2].Value.ToString() == "852573"){
                g.DrawString("BOX DE POCHE 4G MEIG MIXRC", new System.Drawing.Font(new System.Drawing.Font("Arial", 7), FontStyle.Regular), brush, new System.Drawing.PointF(194.5f, 135));
            }            
    }
    
    public void renderAndPrint(object sender, PrintPageEventArgs e){

        String description;
        String ean = "";

        System.Drawing.Rectangle headerTable = new System.Drawing.Rectangle
        {
            //Size = new System.Drawing.Size(295, 10),
            Size = new System.Drawing.Size(365, 10),
            Location = new System.Drawing.Point(20, 80),
        };

        Graphics g = e.Graphics;
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
        e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
        e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

        var imageLogo = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\Logo_Netceed.png");
        //var imageLogo = System.Drawing.Image.FromFile(@"C:\Program Files\Label Generator\icons\Logo_Netceed.png");
        //var imageLogo = System.Drawing.Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\logo_ietc.png");

        var widthImageLogo = imageLogo.Width;
        var heightImageLogo = imageLogo.Height;

        SolidBrush brushTableColor = new SolidBrush(System.Drawing.Color.LightGray);
        g.FillRectangle(brushTableColor, headerTable);

        System.Drawing.Font bodyFont = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
        System.Drawing.Font titleFont = new System.Drawing.Font("Arial", 6.5f);
        //System.Drawing.Font titleFont = new System.Drawing.Font("Arial", 6.5f, FontStyle.Bold);

        Pen penRectangle = new Pen(System.Drawing.Color.Black, 2);
        SolidBrush brush = new SolidBrush(System.Drawing.Color.Black);

        int x = 35;
        int y = 80;

        if (idPalletModelComboBox.Text == "Box de Poche 4G"){
            ean = "6902176038501";
        }else if (idPalletModelComboBox.Text == "Repeteaur AX1800"){
            ean = "6971571800204";
        }else if (idPalletModelComboBox.Text == "Box de Poche 4G MEIG"){
            ean = "6974889940044";
        }

        System.Drawing.Image barcode1 = Code128Rendering.MakeBarcodeImage(ean, 1, false);
        float heightBarcode1 = barcode1.Height;
        float widthBarcode1  = barcode1.Width;

        g.DrawString("Group Box ID: GB-" + dataLabel[currentPageIndex].groupBoxId, bodyFont, brush, new System.Drawing.PointF(20, y - 18));
        
           
        if(currentPageIndex == 9){
            g.DrawString("Group Box: " + (currentPageIndex + 1) + "/" + dataLabel[currentPageIndex].numberPallets, bodyFont, brush, new System.Drawing.PointF(286, y - 18));
        }else{
            g.DrawString("Group Box: " + (currentPageIndex + 1) + "/" + dataLabel[currentPageIndex].numberPallets, bodyFont, brush, new System.Drawing.PointF(294, y - 18));
        }

        g.DrawString(dataLabel[currentPageIndex].quantity.ToString(), bodyFont, brush, new System.Drawing.PointF(x, 108));
        g.DrawString(dataLabel[currentPageIndex].codeArticle.ToString(), bodyFont, brush, new System.Drawing.PointF(x + 52, 108.7f));
          
        g.DrawImage(imageLogo, 10, 15, (float)(widthImageLogo*0.10), (float)(heightImageLogo*0.10));
        g.DrawRectangle(penRectangle , headerTable);
        g.DrawString("Qt", titleFont, brush, new System.Drawing.PointF(x, y));
        g.DrawLine(penRectangle, 65, 80, 65, 90); 
        g.DrawString("Ref.Client", titleFont, brush, new System.Drawing.PointF(x + 52, y));
        g.DrawLine(penRectangle, 155, 80, 155, 90);  
        g.DrawString("Description", titleFont, brush, new System.Drawing.PointF(242, y));

        y += 30;

        g.DrawImage(barcode1, 197, y - 12, widthBarcode1 * 0.85f, heightBarcode1 * 0.85f);
        g.DrawString(ean, new System.Drawing.Font(new System.Drawing.Font("Arial", 5.5f), FontStyle.Regular), brush, new System.Drawing.PointF(239.5f, 122));
        
        if (dataLabel[currentPageIndex].codeArticle.ToString() == "852495"){
            g.DrawString("BOX DE POCHE 4G LEGO MIXRC", new System.Drawing.Font(new System.Drawing.Font("Arial", 7), FontStyle.Regular), brush, new System.Drawing.PointF(194.5f, 135));
        }else if (dataLabel[currentPageIndex].codeArticle.ToString() == "852501"){
            g.DrawString("REPETEUR AX1800 SDMC MIXRC", new System.Drawing.Font(new System.Drawing.Font("Arial", 7), FontStyle.Regular), brush, new System.Drawing.PointF(194.5f, 135));
        }else if (dataLabel[currentPageIndex].codeArticle.ToString() == "852573"){
            g.DrawString("BOX DE POCHE 4G MEIG MIXRC", new System.Drawing.Font(new System.Drawing.Font("Arial", 7), FontStyle.Regular), brush, new System.Drawing.PointF(194.5f, 135));
        }
        
        currentPageIndex++;

        e.HasMorePages = currentPageIndex < dataLabel.Count;
    }

    public void Print(){
        PrintDocument pd = new PrintDocument();
        
        try{
            pd.DefaultPageSettings.PrinterSettings.PrinterName = printersListComboBox.Text;

            pd.PrintPage += new PrintPageEventHandler(renderAndPrint);
            
            pd.Print();

            currentPageIndex = 0;
            
            dataLabel.Clear();
            
        
        }catch(InvalidPrinterException e){
            MessageBox.Show("Alguma coisa inesperada ocorreu com a impressora ! ", "Erro impressora", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }   

    public void PrintRecorFromDataGrid(){
        PrintDocument pd = new PrintDocument();
        
        try{
            pd.DefaultPageSettings.PrinterSettings.PrinterName = printersListComboBox.Text;

            pd.PrintPage += new PrintPageEventHandler(renderAndPrintRecordEdited);
            
            pd.Print(); 
        }catch(InvalidPrinterException e){
            MessageBox.Show("Alguma coisa inesperada ocorreu com a impressora ! ", "Erro impressora", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }   

    private void dataGridViewPallet_MouseClick(object sender, MouseEventArgs e){
        if (e.Button == MouseButtons.Right){
            contextMenu.Show(Cursor.Position.X, Cursor.Position.Y);
        }
    }    
}
        
    

