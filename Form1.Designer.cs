
namespace pocketlabeldata;

using System.Drawing;

partial class Form1
{  
    public TabControl tabControl = new TabControl();
    public TabPage refurbTabPage = new TabPage();
    public TabPage refurbGroupingLabelTabPage = new TabPage();
    ComboBox modelComboBox = new ComboBox();
    public Label labelMacRefurbPage = new Label();
    public TextBox serialBoxRefurbPage = new TextBox();
    public TextBox macBoxRefurbPage = new TextBox();
    Label qrCodeLabel = new Label();
    TextBox qrCodeBox = new TextBox();
    Button callWebService = new Button();
    Button printButton = new Button();          
    CheckBox printAutomatic = new CheckBox();
    Label autoPrintLabel = new Label();
    Button closeButtonForm = new Button();   
    Label serialLabelRefurbPage = new Label();
    public TextBox dataResponse = new TextBox();
    public Label modelLabel = new Label();
    public Label counterNumberPallets = new Label();
    public Label numberPalletsLabel = new Label();
    public TextBox numberPalletsBox = new TextBox();
    public CheckBox insertQuantityManually = new CheckBox();
    public Label idPalletLabel = new Label();
    public TextBox idPalletBox = new TextBox();
    public Button insertIdPalletValues = new Button();
    public Label idPalletModelLabel = new Label();
    public ComboBox idPalletModelComboBox = new ComboBox();
    public Label printDestiny = new Label();
    public ComboBox printersListComboBox = new ComboBox();
    public Button printButtonLabelIdPallet = new Button();
    public DataGridView dataGridViewPallet = new DataGridView();
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {   

        tabControl.Location = new Point(15, 15);
        tabControl.Size = new System.Drawing.Size(1125, 620);
        StartPosition = FormStartPosition.CenterScreen;

        modelLabel.Text = "MODELO: ";
        modelLabel.AutoSize = true;
        modelLabel.Font = new Font("Arial", 13, FontStyle.Bold);
        modelLabel.Location = new Point(45, 40);

        modelComboBox.Location = new Point(200, 40);
        modelComboBox.Font = new Font("Arial", 13, FontStyle.Bold);
        modelComboBox.Width = 500;
        modelComboBox.DropDownHeight = 150;

         /* Etiqueta para o serial */
        serialLabelRefurbPage.Text = "SERIAL: ";
        serialLabelRefurbPage.AutoSize = true;
        serialLabelRefurbPage.Font = new Font("Arial", 13, FontStyle.Bold);
        serialLabelRefurbPage.Location = new Point(45, 130);

        /* Caixa de texto para o serial */
        serialBoxRefurbPage.Width = 500;
        serialBoxRefurbPage.AcceptsTab = true;
        serialBoxRefurbPage.Font = new Font(Font.FontFamily, 16);
        serialBoxRefurbPage.Location = new Point(200, 120);
        //serialBoxRefurbPage.Text = "Scanear o Serial aqui...";

        labelMacRefurbPage.Text = "MAC: ";
        labelMacRefurbPage.AutoSize = true;
        labelMacRefurbPage.Font = new Font("Arial", 13, FontStyle.Bold);
        labelMacRefurbPage.Visible = false;

        macBoxRefurbPage.Width = 500;
        macBoxRefurbPage.AcceptsTab = true;
        macBoxRefurbPage.Font = new Font(Font.FontFamily, 16);
        macBoxRefurbPage.Visible = false;

        qrCodeLabel.Text = "QR CODE:";
        qrCodeLabel.AutoSize = true;
        qrCodeLabel.Font = new Font("Arial", 13, FontStyle.Bold);
        qrCodeLabel.Location = new Point(45, 205);
        qrCodeLabel.Visible = false;

        qrCodeBox.Width = 500;
        qrCodeBox.AcceptsTab = true;
        qrCodeBox.Font = new Font(qrCodeBox.Font.FontFamily, 16);
        qrCodeBox.Location = new Point(200, 197);
        qrCodeBox.Text = "Scanear o QR Code aqui...";
        qrCodeBox.Visible = false;

        dataResponse.Multiline = true;
        dataResponse.Font = new Font(Font.FontFamily, 20, FontStyle.Bold);
        dataResponse.Location = new Point(45, 250);
        dataResponse.Width = 660;
        dataResponse.Height = 285;
        dataResponse.Enabled = true;
        dataResponse.TabStop = false;
        dataResponse.ReadOnly = true;
        dataResponse.ScrollBars = ScrollBars.Both;
        dataResponse.WordWrap = false;

        callWebService.Image = Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\right-arrow.png");
        //callWebService.Image = Image.FromFile(@"C:\Program Files\Label Generator\icons\right-arrow.png");
        callWebService.BackgroundImageLayout = ImageLayout.Stretch;
        callWebService.Width = 50;
        callWebService.Height = 53;
        callWebService.Location = new Point(720,118);

        closeButtonForm.Image = Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\exit_icon.ico"); 
        //closeButtonForm.Image = Image.FromFile(@"C:\Program Files\Label Generator\icons\exit_icon.ico"); 
        closeButtonForm.BackgroundImageLayout = ImageLayout.Stretch;
        closeButtonForm.Width = 50;
        closeButtonForm.Height = 50;
        closeButtonForm.Location = new Point(1150, 10);
        closeButtonForm.BackColor = Color.Transparent;

        printButton.Image = Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\print.png");
        //printButton.Image = Image.FromFile(@"C:\Program Files\Label Generator\icons\print.png");
        printButton.Width = 100;
        printButton.Height = 100;
        printButton.BackgroundImageLayout = ImageLayout.Stretch;
        printButton.Location = new Point(815, 320);

        autoPrintLabel.Text = "Impressão\nAutomática";
        autoPrintLabel.AutoSize = true;
        autoPrintLabel.Font = new Font("Arial", 13, FontStyle.Bold);
        autoPrintLabel.Location = new Point(792, 440);

        printAutomatic.Location = new Point(855, 510);

        refurbTabPage.Text = "Refurb Labels";
        refurbGroupingLabelTabPage.Text = "ID Palete Individual Label";

        idPalletModelLabel.Text = "MODELO: ";
        idPalletModelLabel.AutoSize = true;
        idPalletModelLabel.Font = new Font("Arial", 13, FontStyle.Bold);
        idPalletModelLabel.Location = new Point(50, 30);

        idPalletModelComboBox.Width = 450;
        idPalletModelComboBox.Font = new Font("Arial", 16, FontStyle.Bold);
        idPalletModelComboBox.Location = new Point(330, 25);
        //idPalletModelComboBox.Text = "Box de Poche 4G";

        numberPalletsLabel.Text = "NÚMERO CAIXAS: ";
        numberPalletsLabel.AutoSize = true;
        numberPalletsLabel.Font = new Font("Arial", 13, FontStyle.Bold);
        numberPalletsLabel.Location = new Point(50, 95);

        numberPalletsBox.Width = 450;
        numberPalletsBox.Font = new Font("Arial", 16);
        numberPalletsBox.Location = new Point(330, 95);
        //numberPalletsBox.Text = "1";

        insertQuantityManually.Location = new Point(795, 90);
        insertQuantityManually.Text = "Quantidade manual";
        insertQuantityManually.Height = 50;
        insertQuantityManually.Width = 200;
        insertIdPalletValues.TextAlign = ContentAlignment.BottomCenter;

        //counterNumberPallets.Text = 1 + "/" + numberPalletsBox.Text;
        counterNumberPallets.AutoSize = true;
        counterNumberPallets.Font = new Font("Arial", 16, FontStyle.Bold);
        counterNumberPallets.Location = new Point(840,245);
        counterNumberPallets.Visible = false;

        idPalletLabel.Text = "ID PALETE: ";
        idPalletLabel.AutoSize = true;
        idPalletLabel.Font = new Font("Arial", 13, FontStyle.Bold);
        idPalletLabel.Location = new Point(50, 155);

        idPalletBox.Width = 450;
        idPalletBox.Font = new Font("Arial", 16);
        idPalletBox.Location = new Point(330, 155);
        //idPalletBox.Text = "P-24050000205";

        insertIdPalletValues.Image = Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\right-arrow.png");
        //insertIdPalletValues.Image = Image.FromFile(@"C:\Program Files\Label Generator\icons\right-arrow.png");
        insertIdPalletValues.BackgroundImageLayout = ImageLayout.Stretch;
        insertIdPalletValues.Width = 50;
        insertIdPalletValues.Height = 45;
        insertIdPalletValues.Location = new Point(785,153);
        
        //printButtonLabelIdPallet.Image = Image.FromFile(@"C:\Program Files\Label Generator\icons\print.png");
        printButtonLabelIdPallet.Image = Image.FromFile(@"C:\Users\User\IdeaProjects\pocketlabeldata\icons\print.png");
        printButtonLabelIdPallet.Width = 100;
        printButtonLabelIdPallet.Height = 100;
        printButtonLabelIdPallet.BackgroundImageLayout = ImageLayout.Stretch;
        printButtonLabelIdPallet.Location = new Point(825, 325);

        printDestiny.Text = "IMPRESSORA: ";
        printDestiny.AutoSize = true;
        printDestiny.Font = new Font("Arial", 13, FontStyle.Bold);
        printDestiny.Location = new Point(50, 225);

        printersListComboBox.Width = 450;
        printersListComboBox.Font = new Font("Arial", 13, FontStyle.Bold);
        printersListComboBox.Location = new Point(330, 225);

        dataGridViewPallet.Location = new Point(55, 320);
        dataGridViewPallet.Size = new Size(730, 250);
            
        dataGridViewPallet.ColumnHeadersBorderStyle =
            DataGridViewHeaderBorderStyle.Single;

        dataGridViewPallet.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        dataGridViewPallet.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        dataGridViewPallet.GridColor = Color.Black;

        dataGridViewPallet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewPallet.MultiSelect = false;

        dataGridViewPallet.AllowUserToAddRows = false;
        dataGridViewPallet.AllowUserToDeleteRows = false;
        dataGridViewPallet.AllowUserToResizeRows = false;

        tabControl.TabPages.AddRange([refurbTabPage, refurbGroupingLabelTabPage]);

        this.Controls.AddRange([tabControl, closeButtonForm]);

        refurbTabPage.Controls.AddRange([modelLabel, modelComboBox, serialLabelRefurbPage, serialBoxRefurbPage, labelMacRefurbPage, macBoxRefurbPage, callWebService, printButton, dataResponse, printAutomatic, printAutomatic, autoPrintLabel]);
        refurbGroupingLabelTabPage.Controls.AddRange([idPalletModelLabel, idPalletModelComboBox, counterNumberPallets, numberPalletsLabel, numberPalletsBox, insertQuantityManually, idPalletLabel, idPalletBox, insertIdPalletValues, printDestiny, printersListComboBox, dataGridViewPallet, printButtonLabelIdPallet]);

        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1200, 650);

        //tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
        idPalletModelComboBox.SelectedIndexChanged += idPalletModelComboBox_SelectedIndexChanged;

        printButton.Click += new System.EventHandler(PrintButton_click);
        printButtonLabelIdPallet.Click += new System.EventHandler(printButtonLabelIdPallet_click);

        closeButtonForm.Click += new System.EventHandler(formClose_click);

        //passwordForm.buttonSubmitPassword.Click += buttonSubmitPassword_Click;
        
        callWebService.Enter += callWebService_OnFocus;
        callWebService.GotFocus += callWebService_OnFocus; 
        printButton.GotFocus += printButton_OnFocus;
        insertIdPalletValues.GotFocus += insertIdPalletValues_OnFocus;

        numberPalletsBox.Leave += numberPalletsBox_Leave;

        dataGridViewPallet.MouseDown += dataGridViewPallet_MouseClick;

    }

    #endregion

}
