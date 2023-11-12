
namespace InvestmentManagement
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
      this.btn_AktualisiereAktienkurse = new System.Windows.Forms.Button();
      this.txt_StatusLeiste = new System.Windows.Forms.TextBox();
      this.btn_AktualisiereCryptosPreise = new System.Windows.Forms.Button();
      this.btn_BerKaufCryptos = new System.Windows.Forms.Button();
      this.txtb_LineNewBuy = new System.Windows.Forms.TextBox();
      this.lbl_LineNewBuy = new System.Windows.Forms.Label();
      this.btn_CollBuySellCryptos = new System.Windows.Forms.Button();
      this.btn_MergeFlies = new System.Windows.Forms.Button();
      this.btn_CreateCryptoRegister = new System.Windows.Forms.Button();
      this.btn_GetNetworkScannerTransactions = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // btn_AktualisiereAktienkurse
      // 
      this.btn_AktualisiereAktienkurse.Location = new System.Drawing.Point(104, 101);
      this.btn_AktualisiereAktienkurse.Name = "btn_AktualisiereAktienkurse";
      this.btn_AktualisiereAktienkurse.Size = new System.Drawing.Size(141, 35);
      this.btn_AktualisiereAktienkurse.TabIndex = 0;
      this.btn_AktualisiereAktienkurse.Text = "Aktualisiere Aktienkurse";
      this.btn_AktualisiereAktienkurse.UseVisualStyleBackColor = true;
      this.btn_AktualisiereAktienkurse.Click += new System.EventHandler(this.Click_RefreshStockPrice);
      // 
      // txt_StatusLeiste
      // 
      this.txt_StatusLeiste.Location = new System.Drawing.Point(12, 418);
      this.txt_StatusLeiste.Name = "txt_StatusLeiste";
      this.txt_StatusLeiste.Size = new System.Drawing.Size(567, 20);
      this.txt_StatusLeiste.TabIndex = 1;
      // 
      // btn_AktualisiereCryptosPreise
      // 
      this.btn_AktualisiereCryptosPreise.Location = new System.Drawing.Point(353, 101);
      this.btn_AktualisiereCryptosPreise.Name = "btn_AktualisiereCryptosPreise";
      this.btn_AktualisiereCryptosPreise.Size = new System.Drawing.Size(160, 44);
      this.btn_AktualisiereCryptosPreise.TabIndex = 2;
      this.btn_AktualisiereCryptosPreise.Text = "Aktualisiere Cryptos Preise";
      this.btn_AktualisiereCryptosPreise.UseVisualStyleBackColor = true;
      this.btn_AktualisiereCryptosPreise.Click += new System.EventHandler(this.Click_RefreshCryptoPrice);
      // 
      // btn_BerKaufCryptos
      // 
      this.btn_BerKaufCryptos.Location = new System.Drawing.Point(353, 164);
      this.btn_BerKaufCryptos.Name = "btn_BerKaufCryptos";
      this.btn_BerKaufCryptos.Size = new System.Drawing.Size(160, 44);
      this.btn_BerKaufCryptos.TabIndex = 3;
      this.btn_BerKaufCryptos.Text = "Berechne Kauf von Cryptos";
      this.btn_BerKaufCryptos.UseVisualStyleBackColor = true;
      this.btn_BerKaufCryptos.Click += new System.EventHandler(this.Click_CalculateBuyOfCryptos);
      // 
      // txtb_LineNewBuy
      // 
      this.txtb_LineNewBuy.Location = new System.Drawing.Point(629, 164);
      this.txtb_LineNewBuy.Name = "txtb_LineNewBuy";
      this.txtb_LineNewBuy.Size = new System.Drawing.Size(100, 20);
      this.txtb_LineNewBuy.TabIndex = 4;
      // 
      // lbl_LineNewBuy
      // 
      this.lbl_LineNewBuy.AutoSize = true;
      this.lbl_LineNewBuy.Location = new System.Drawing.Point(520, 164);
      this.lbl_LineNewBuy.Name = "lbl_LineNewBuy";
      this.lbl_LineNewBuy.Size = new System.Drawing.Size(103, 13);
      this.lbl_LineNewBuy.TabIndex = 5;
      this.lbl_LineNewBuy.Text = "Zeile ab neuer Kauf:";
      // 
      // btn_CollBuySellCryptos
      // 
      this.btn_CollBuySellCryptos.Location = new System.Drawing.Point(353, 274);
      this.btn_CollBuySellCryptos.Name = "btn_CollBuySellCryptos";
      this.btn_CollBuySellCryptos.Size = new System.Drawing.Size(160, 44);
      this.btn_CollBuySellCryptos.TabIndex = 6;
      this.btn_CollBuySellCryptos.Text = "Sammel Kauf/Verakuf Dataen von Cryptos";
      this.btn_CollBuySellCryptos.UseVisualStyleBackColor = true;
      this.btn_CollBuySellCryptos.Click += new System.EventHandler(this.Click_btn_CollBuySellCryptos);
      // 
      // btn_MergeFlies
      // 
      this.btn_MergeFlies.Location = new System.Drawing.Point(353, 214);
      this.btn_MergeFlies.Name = "btn_MergeFlies";
      this.btn_MergeFlies.Size = new System.Drawing.Size(160, 44);
      this.btn_MergeFlies.TabIndex = 7;
      this.btn_MergeFlies.Text = "Führe Dokumente zusammen";
      this.btn_MergeFlies.UseVisualStyleBackColor = true;
      this.btn_MergeFlies.Click += new System.EventHandler(this.Click_btn_MergeFlies);
      // 
      // btn_CreateCryptoRegister
      // 
      this.btn_CreateCryptoRegister.Location = new System.Drawing.Point(353, 324);
      this.btn_CreateCryptoRegister.Name = "btn_CreateCryptoRegister";
      this.btn_CreateCryptoRegister.Size = new System.Drawing.Size(160, 44);
      this.btn_CreateCryptoRegister.TabIndex = 8;
      this.btn_CreateCryptoRegister.Text = "Create Crypto Register";
      this.btn_CreateCryptoRegister.UseVisualStyleBackColor = true;
      this.btn_CreateCryptoRegister.Click += new System.EventHandler(this.Click_btn_CreateCryptoRegister);
      // 
      // btn_GetNetworkScannerTransactions
      // 
      this.btn_GetNetworkScannerTransactions.Location = new System.Drawing.Point(353, 374);
      this.btn_GetNetworkScannerTransactions.Name = "btn_GetNetworkScannerTransactions";
      this.btn_GetNetworkScannerTransactions.Size = new System.Drawing.Size(160, 44);
      this.btn_GetNetworkScannerTransactions.TabIndex = 9;
      this.btn_GetNetworkScannerTransactions.Text = "Get Network Scanner Transactions";
      this.btn_GetNetworkScannerTransactions.UseVisualStyleBackColor = true;
      this.btn_GetNetworkScannerTransactions.Click += new System.EventHandler(this.Click_btn_GetNetworkScannerTransactions);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.btn_GetNetworkScannerTransactions);
      this.Controls.Add(this.btn_CreateCryptoRegister);
      this.Controls.Add(this.btn_MergeFlies);
      this.Controls.Add(this.btn_CollBuySellCryptos);
      this.Controls.Add(this.lbl_LineNewBuy);
      this.Controls.Add(this.txtb_LineNewBuy);
      this.Controls.Add(this.btn_BerKaufCryptos);
      this.Controls.Add(this.btn_AktualisiereCryptosPreise);
      this.Controls.Add(this.txt_StatusLeiste);
      this.Controls.Add(this.btn_AktualisiereAktienkurse);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_AktualisiereAktienkurse;
        private System.Windows.Forms.TextBox txt_StatusLeiste;
    private System.Windows.Forms.Button btn_AktualisiereCryptosPreise;
    private System.Windows.Forms.Button btn_BerKaufCryptos;
    private System.Windows.Forms.TextBox txtb_LineNewBuy;
    private System.Windows.Forms.Label lbl_LineNewBuy;
    private System.Windows.Forms.Button btn_CollBuySellCryptos;
    private System.Windows.Forms.Button btn_MergeFlies;
    private System.Windows.Forms.Button btn_CreateCryptoRegister;
    protected System.Windows.Forms.Button btn_GetNetworkScannerTransactions;
  }
}

