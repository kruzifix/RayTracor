namespace RayTracor
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.renderControl1 = new RayTracor.RenderControl();
            this.bRender = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // renderControl1
            // 
            this.renderControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderControl1.Location = new System.Drawing.Point(12, 41);
            this.renderControl1.Name = "renderControl1";
            this.renderControl1.Size = new System.Drawing.Size(527, 366);
            this.renderControl1.TabIndex = 0;
            // 
            // bRender
            // 
            this.bRender.Location = new System.Drawing.Point(12, 12);
            this.bRender.Name = "bRender";
            this.bRender.Size = new System.Drawing.Size(75, 23);
            this.bRender.TabIndex = 1;
            this.bRender.Text = "Render";
            this.bRender.UseVisualStyleBackColor = true;
            this.bRender.Click += new System.EventHandler(this.bRender_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 419);
            this.Controls.Add(this.bRender);
            this.Controls.Add(this.renderControl1);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private RayTracor.RenderControl renderControl1;
        private System.Windows.Forms.Button bRender;



    }
}

