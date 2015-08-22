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
            this.bSave = new System.Windows.Forms.Button();
            this.bLoad = new System.Windows.Forms.Button();
            this.bDepthMap = new System.Windows.Forms.Button();
            this.bNormalMap = new System.Windows.Forms.Button();
            this.bAO = new System.Windows.Forms.Button();
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.cBoxSuSas = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cBoxSuSaMode = new System.Windows.Forms.ComboBox();
            this.renderControl = new RayTracor.RenderControl();
            this.bSSAO = new System.Windows.Forms.Button();
            this.bRenderAO = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bRender
            // 
            this.bRender.Location = new System.Drawing.Point(193, 41);
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
            this.bDepthMap.Location = new System.Drawing.Point(453, 41);
            this.bDepthMap.Name = "bDepthMap";
            this.bDepthMap.Size = new System.Drawing.Size(75, 23);
            this.bDepthMap.TabIndex = 8;
            this.bDepthMap.Text = "Depth Map";
            this.bDepthMap.UseVisualStyleBackColor = true;
            this.bDepthMap.Click += new System.EventHandler(this.bDepthMap_Click);
            // 
            // bNormalMap
            // 
            this.bNormalMap.Location = new System.Drawing.Point(372, 41);
            this.bNormalMap.Name = "bNormalMap";
            this.bNormalMap.Size = new System.Drawing.Size(75, 23);
            this.bNormalMap.TabIndex = 9;
            this.bNormalMap.Text = "Normal Map";
            this.bNormalMap.UseVisualStyleBackColor = true;
            this.bNormalMap.Click += new System.EventHandler(this.bNormalMap_Click);
            // 
            // bAO
            // 
            this.bAO.Location = new System.Drawing.Point(534, 41);
            this.bAO.Name = "bAO";
            this.bAO.Size = new System.Drawing.Size(104, 23);
            this.bAO.TabIndex = 10;
            this.bAO.Text = "Ambient Occlusion";
            this.bAO.UseVisualStyleBackColor = true;
            this.bAO.Click += new System.EventHandler(this.bAO_Click);
            // 
            // progBar
            // 
            this.progBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progBar.Location = new System.Drawing.Point(15, 70);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(713, 23);
            this.progBar.Step = 1;
            this.progBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progBar.TabIndex = 11;
            // 
            // cBoxSuSas
            // 
            this.cBoxSuSas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxSuSas.FormattingEnabled = true;
            this.cBoxSuSas.Location = new System.Drawing.Point(291, 14);
            this.cBoxSuSas.Name = "cBoxSuSas";
            this.cBoxSuSas.Size = new System.Drawing.Size(52, 21);
            this.cBoxSuSas.TabIndex = 13;
            this.cBoxSuSas.SelectedIndexChanged += new System.EventHandler(this.cBoxSuSas_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "SuperSamples:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(349, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Mode:";
            // 
            // cBoxSuSaMode
            // 
            this.cBoxSuSaMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxSuSaMode.FormattingEnabled = true;
            this.cBoxSuSaMode.Location = new System.Drawing.Point(392, 14);
            this.cBoxSuSaMode.Name = "cBoxSuSaMode";
            this.cBoxSuSaMode.Size = new System.Drawing.Size(84, 21);
            this.cBoxSuSaMode.TabIndex = 15;
            this.cBoxSuSaMode.SelectedIndexChanged += new System.EventHandler(this.cBoxSuSaMode_SelectedIndexChanged);
            // 
            // renderControl
            // 
            this.renderControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderControl.Location = new System.Drawing.Point(15, 99);
            this.renderControl.Name = "renderControl";
            this.renderControl.Size = new System.Drawing.Size(713, 408);
            this.renderControl.TabIndex = 0;
            this.renderControl.Click += new System.EventHandler(this.renderControl_Click);
            // 
            // bSSAO
            // 
            this.bSSAO.Location = new System.Drawing.Point(644, 41);
            this.bSSAO.Name = "bSSAO";
            this.bSSAO.Size = new System.Drawing.Size(75, 23);
            this.bSSAO.TabIndex = 16;
            this.bSSAO.Text = "SSAO";
            this.bSSAO.UseVisualStyleBackColor = true;
            this.bSSAO.Click += new System.EventHandler(this.bSSAO_Click);
            // 
            // bRenderAO
            // 
            this.bRenderAO.Location = new System.Drawing.Point(274, 41);
            this.bRenderAO.Name = "bRenderAO";
            this.bRenderAO.Size = new System.Drawing.Size(92, 23);
            this.bRenderAO.TabIndex = 17;
            this.bRenderAO.Text = "Render w/ AO";
            this.bRenderAO.UseVisualStyleBackColor = true;
            this.bRenderAO.Click += new System.EventHandler(this.bRenderAO_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 519);
            this.Controls.Add(this.bRenderAO);
            this.Controls.Add(this.bSSAO);
            this.Controls.Add(this.cBoxSuSaMode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cBoxSuSas);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progBar);
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
        private System.Windows.Forms.ProgressBar progBar;
        private System.Windows.Forms.ComboBox cBoxSuSas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cBoxSuSaMode;
        private System.Windows.Forms.Button bSSAO;
        private System.Windows.Forms.Button bRenderAO;
    }
}

