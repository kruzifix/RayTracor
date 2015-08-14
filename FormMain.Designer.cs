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
            this.bRenderParallel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cBoxTasks = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cBoxResolution = new System.Windows.Forms.ComboBox();
            this.renderControl = new RayTracor.RenderControl();
            this.bSave = new System.Windows.Forms.Button();
            this.bLoad = new System.Windows.Forms.Button();
            this.bRenderDepth = new System.Windows.Forms.Button();
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
            // bRenderParallel
            // 
            this.bRenderParallel.Location = new System.Drawing.Point(408, 12);
            this.bRenderParallel.Name = "bRenderParallel";
            this.bRenderParallel.Size = new System.Drawing.Size(87, 23);
            this.bRenderParallel.TabIndex = 2;
            this.bRenderParallel.Text = "Render Parallel";
            this.bRenderParallel.UseVisualStyleBackColor = true;
            this.bRenderParallel.Click += new System.EventHandler(this.bRenderParallel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(288, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tasks:";
            // 
            // cBoxTasks
            // 
            this.cBoxTasks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxTasks.FormattingEnabled = true;
            this.cBoxTasks.Location = new System.Drawing.Point(333, 14);
            this.cBoxTasks.Name = "cBoxTasks";
            this.cBoxTasks.Size = new System.Drawing.Size(69, 21);
            this.cBoxTasks.TabIndex = 4;
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
            // bRenderDepth
            // 
            this.bRenderDepth.Location = new System.Drawing.Point(207, 41);
            this.bRenderDepth.Name = "bRenderDepth";
            this.bRenderDepth.Size = new System.Drawing.Size(83, 23);
            this.bRenderDepth.TabIndex = 8;
            this.bRenderDepth.Text = "Render Depth";
            this.bRenderDepth.UseVisualStyleBackColor = true;
            this.bRenderDepth.Click += new System.EventHandler(this.bRenderDepth_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 419);
            this.Controls.Add(this.bRenderDepth);
            this.Controls.Add(this.bLoad);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.cBoxResolution);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cBoxTasks);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bRenderParallel);
            this.Controls.Add(this.bRender);
            this.Controls.Add(this.renderControl);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RayTracor.RenderControl renderControl;
        private System.Windows.Forms.Button bRender;
        private System.Windows.Forms.Button bRenderParallel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cBoxTasks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cBoxResolution;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.Button bRenderDepth;
    }
}

