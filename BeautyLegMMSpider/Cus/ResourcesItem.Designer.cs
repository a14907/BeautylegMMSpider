namespace BeautyLegMMSpider.Cus
{
    partial class ResourcesItem
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chboxUrl = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chboxUrl
            // 
            this.chboxUrl.AutoSize = true;
            this.chboxUrl.Location = new System.Drawing.Point(4, 4);
            this.chboxUrl.Name = "chboxUrl";
            this.chboxUrl.Size = new System.Drawing.Size(15, 14);
            this.chboxUrl.TabIndex = 0;
            this.chboxUrl.UseVisualStyleBackColor = true;
            // 
            // ResourcesItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.chboxUrl);
            this.Location = new System.Drawing.Point(10, 20);
            this.Name = "ResourcesItem";
            this.Size = new System.Drawing.Size(395, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chboxUrl;
    }
}
