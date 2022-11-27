
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
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
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
  }
}

