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
            this.bRender = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cBoxResolution = new System.Windows.Forms.ComboBox();
            this.renderControl = new RayTracor.RenderControl();
            this.bSave = new System.Windows.Forms.Button();
            this.bLoad = new System.Windows.Forms.Button();
            this.bDepthMap = new System.Windows.Forms.Button();
            this.bNormalMap = new System.Windows.Forms.Button();
            this.bAO = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bRender
            // 
            this.bRender.Location = new System.Drawing.Point(207, 12);
            this.bRender.Name = "bRender";
            this.bRender.Size = new System.Drawing.Size(75, 23);
            this.bRender.TabIndex = 1;
            this.bRender.Text = "Render";
            this.bRender.UseVisualStyleBackColor = true;
            this.bRender.Click += new System.EventHandler(this.bRender_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Resolution:";
            // 
            // cBoxResolution
            // 
            this.cBoxResolution.FormattingEnabled = true;
            this.cBoxResolution.Location = new System.Drawing.Point(78, 14);
            this.cBoxResolution.Name = "cBoxResolution";
            this.cBoxResolution.Size = new System.Drawing.Size(123, 21);
            this.cBoxResolution.TabIndex = 6;
            this.cBoxResolution.TextUpdate += new System.EventHandler(this.cBoxResolution_TextUpdate);
            this.cBoxResolution.TextChanged += new System.EventHandler(this.cBoxResolution_TextUpdate);
            // 
            // renderControl
            // 
            this.renderControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderControl.Location = new System.Drawing.Point(12, 84);
            this.renderControl.Name = "renderControl";
            this.renderControl.Size = new System.Drawing.Size(527, 323);
            this.renderControl.TabIndex = 0;
            this.renderControl.Click += new System.EventHandler(this.renderControl_Click);
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(15, 41);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 23);
            this.bSave.TabIndex = 7;
            this.bSave.Text = "Save Scene";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bLoad
            // 
            this.bLoad.Location = new System.Drawing.Point(96, 41);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(75, 23);
            this.bLoad.TabIndex = 7;
            this.bLoad.Text = "Load Scene";
            this.bLoad.UseVisualStyleBackColor = true;
            this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // bDepthMap
            // 
            this.bDepthMap.Location = new System.Drawing.Point(207, 41);
            this.bDepthMap.Name = "bDepthMap";
            this.bDepthMap.Size = new System.Drawing.Size(75, 23);
            this.bDepthMap.TabIndex = 8;
            this.bDepthMap.Text = "Depth Map";
            this.bDepthMap.UseVisualStyleBackColor = true;
            this.bDepthMap.Click += new System.EventHandler(this.bDepthMap_Click);
            // 
            // bNormalMap
            // 
            this.bNormalMap.Location = new System.Drawing.Point(288, 41);
            this.bNormalMap.Name = "bNormalMap";
            this.bNormalMap.Size = new System.Drawing.Size(75, 23);
            this.bNormalMap.TabIndex = 9;
            this.bNormalMap.Text = "Normal Map";
            this.bNormalMap.UseVisualStyleBackColor = true;
            this.bNormalMap.Click += new System.EventHandler(this.bNormalMap_Click);
            // 
            // bAO
            // 
            this.bAO.Location = new System.Drawing.Point(369, 41);
            this.bAO.Name = "bAO";
            this.bAO.Size = new System.Drawing.Size(104, 23);
            this.bAO.TabIndex = 10;
            this.bAO.Text = "Ambient Occlusion";
            this.bAO.UseVisualStyleBackColor = true;
            this.bAO.Click += new System.EventHandler(this.bAO_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 419);
            this.Controls.Add(this.bAO);
            this.Controls.Add(this.bNormalMap);
            this.Controls.Add(this.bDepthMap);
            this.Controls.Add(this.bLoad);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.cBoxResolution);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bRender);
            this.Controls.Add(this.renderControl);
            this.Name = "FormMain";
            this.Text = "Ray Tracor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RayTracor.RenderControl renderControl;
        private System.Windows.Forms.Button bRender;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cBoxResolution;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.Button bDepthMap;
        private System.Windows.Forms.Button bNormalMap;
        private System.Windows.Forms.Button bAO;
    }
}

