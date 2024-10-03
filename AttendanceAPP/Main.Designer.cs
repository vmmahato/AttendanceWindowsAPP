namespace AttendanceAPP_CRUD
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btn_add = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.IPAddress = new System.Windows.Forms.TextBox();
            this.device_uuid = new System.Windows.Forms.TextBox();
            this.port = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_update = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.DeviceName = new System.Windows.Forms.Label();
            this.device_name = new System.Windows.Forms.TextBox();
            this.AccessCtrlUUID = new System.Windows.Forms.TextBox();
            this.PING = new System.Windows.Forms.Button();
            this.IsManual = new System.Windows.Forms.CheckBox();
            this.txt_SerailNumber = new System.Windows.Forms.TextBox();
            this.IsActive = new System.Windows.Forms.CheckBox();
            this.IsAccessControl = new System.Windows.Forms.CheckBox();
            this.IsAttendance = new System.Windows.Forms.CheckBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.LastFetchDateTime = new System.Windows.Forms.Label();
            this.DataBackup = new System.Windows.Forms.Button();
            this.ImportDatabase = new System.Windows.Forms.Button();
            this.SEARCH = new System.Windows.Forms.Button();
            this.CompanyName = new System.Windows.Forms.Label();
            this.Company_Name = new System.Windows.Forms.TextBox();
            this.EmailAddress = new System.Windows.Forms.Label();
            this.Email_Address = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Phone_Number = new System.Windows.Forms.TextBox();
            this.ServiceRunTime = new System.Windows.Forms.Label();
            this.Minute = new System.Windows.Forms.Label();
            this.ScheduleTime = new System.Windows.Forms.NumericUpDown();
            this.notification = new System.Windows.Forms.Button();
            this.ActiveNotification = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleTime)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(242, 717);
            this.btn_add.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(115, 44);
            this.btn_add.TabIndex = 16;
            this.btn_add.Text = "ADD";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.Add);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AccessibleName = "ZkTeco";
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(98, 286);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(1840, 401);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GetIdToDelete);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Edit);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10F);
            this.label1.Location = new System.Drawing.Point(585, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 19);
            this.label1.TabIndex = 27;
            this.label1.Text = "IP Address";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // IPAddress
            // 
            this.IPAddress.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPAddress.Location = new System.Drawing.Point(741, 73);
            this.IPAddress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.IPAddress.Name = "IPAddress";
            this.IPAddress.Size = new System.Drawing.Size(265, 30);
            this.IPAddress.TabIndex = 5;
            this.IPAddress.Text = "192.168.1.201";
            this.IPAddress.TextChanged += new System.EventHandler(this.IPAddress_TextChanged);
            // 
            // device_uuid
            // 
            this.device_uuid.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device_uuid.Location = new System.Drawing.Point(741, 117);
            this.device_uuid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.device_uuid.Name = "device_uuid";
            this.device_uuid.Size = new System.Drawing.Size(265, 30);
            this.device_uuid.TabIndex = 10;
            // 
            // port
            // 
            this.port.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.port.Location = new System.Drawing.Point(1269, 66);
            this.port.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(258, 30);
            this.port.TabIndex = 6;
            this.port.Text = "4370";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 10F);
            this.label3.Location = new System.Drawing.Point(1101, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 19);
            this.label3.TabIndex = 28;
            this.label3.Text = "Port No.";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // btn_update
            // 
            this.btn_update.Location = new System.Drawing.Point(385, 717);
            this.btn_update.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(115, 44);
            this.btn_update.TabIndex = 17;
            this.btn_update.Text = "UPDATE";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.Update);
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(526, 717);
            this.btn_delete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(115, 44);
            this.btn_delete.TabIndex = 18;
            this.btn_delete.Text = "DELETE";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.Delete);
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(670, 717);
            this.btn_clear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(115, 44);
            this.btn_clear.TabIndex = 19;
            this.btn_clear.Text = "CLEAR";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.Clear);
            // 
            // DeviceName
            // 
            this.DeviceName.AutoSize = true;
            this.DeviceName.Font = new System.Drawing.Font("Arial", 10F);
            this.DeviceName.Location = new System.Drawing.Point(108, 73);
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.Size = new System.Drawing.Size(106, 19);
            this.DeviceName.TabIndex = 26;
            this.DeviceName.Text = "Device Name";
            this.DeviceName.Click += new System.EventHandler(this.label4_Click);
            // 
            // device_name
            // 
            this.device_name.Font = new System.Drawing.Font("Arial", 12F);
            this.device_name.Location = new System.Drawing.Point(256, 73);
            this.device_name.Name = "device_name";
            this.device_name.Size = new System.Drawing.Size(247, 30);
            this.device_name.TabIndex = 4;
            this.device_name.Text = "Attendance";
            this.device_name.TextChanged += new System.EventHandler(this.device_name_TextChanged);
            // 
            // AccessCtrlUUID
            // 
            this.AccessCtrlUUID.Font = new System.Drawing.Font("Arial", 12F);
            this.AccessCtrlUUID.Location = new System.Drawing.Point(1269, 109);
            this.AccessCtrlUUID.Name = "AccessCtrlUUID";
            this.AccessCtrlUUID.Size = new System.Drawing.Size(258, 30);
            this.AccessCtrlUUID.TabIndex = 12;
            // 
            // PING
            // 
            this.PING.Location = new System.Drawing.Point(95, 717);
            this.PING.Name = "PING";
            this.PING.Size = new System.Drawing.Size(115, 44);
            this.PING.TabIndex = 15;
            this.PING.Text = "PING";
            this.PING.UseVisualStyleBackColor = true;
            this.PING.Click += new System.EventHandler(this.PING_Click);
            // 
            // IsManual
            // 
            this.IsManual.AutoSize = true;
            this.IsManual.Font = new System.Drawing.Font("Arial", 8F);
            this.IsManual.Location = new System.Drawing.Point(112, 125);
            this.IsManual.Name = "IsManual";
            this.IsManual.Size = new System.Drawing.Size(175, 20);
            this.IsManual.TabIndex = 7;
            this.IsManual.Text = "Manual(Serial Number)";
            this.IsManual.UseVisualStyleBackColor = true;
            this.IsManual.CheckedChanged += new System.EventHandler(this.IsManual_CheckedChanged);
            // 
            // txt_SerailNumber
            // 
            this.txt_SerailNumber.Font = new System.Drawing.Font("Arial", 12F);
            this.txt_SerailNumber.Location = new System.Drawing.Point(293, 120);
            this.txt_SerailNumber.Name = "txt_SerailNumber";
            this.txt_SerailNumber.Size = new System.Drawing.Size(210, 30);
            this.txt_SerailNumber.TabIndex = 8;
            // 
            // IsActive
            // 
            this.IsActive.AutoSize = true;
            this.IsActive.Font = new System.Drawing.Font("Arial", 10F);
            this.IsActive.Location = new System.Drawing.Point(112, 167);
            this.IsActive.Name = "IsActive";
            this.IsActive.Size = new System.Drawing.Size(75, 23);
            this.IsActive.TabIndex = 13;
            this.IsActive.Text = "Active";
            this.IsActive.UseVisualStyleBackColor = true;
            // 
            // IsAccessControl
            // 
            this.IsAccessControl.AutoSize = true;
            this.IsAccessControl.Font = new System.Drawing.Font("Arial", 10F);
            this.IsAccessControl.Location = new System.Drawing.Point(1105, 109);
            this.IsAccessControl.Name = "IsAccessControl";
            this.IsAccessControl.Size = new System.Drawing.Size(137, 23);
            this.IsAccessControl.TabIndex = 11;
            this.IsAccessControl.Text = "AccessControl";
            this.IsAccessControl.UseVisualStyleBackColor = true;
            this.IsAccessControl.CheckedChanged += new System.EventHandler(this.IsAccessControl_CheckedChanged);
            // 
            // IsAttendance
            // 
            this.IsAttendance.AutoSize = true;
            this.IsAttendance.Font = new System.Drawing.Font("Arial", 10F);
            this.IsAttendance.Location = new System.Drawing.Point(589, 117);
            this.IsAttendance.Name = "IsAttendance";
            this.IsAttendance.Size = new System.Drawing.Size(113, 23);
            this.IsAttendance.TabIndex = 9;
            this.IsAttendance.Text = "Attendance";
            this.IsAttendance.UseVisualStyleBackColor = true;
            this.IsAttendance.CheckedChanged += new System.EventHandler(this.IsAttendance_CheckedChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Arial", 8F);
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Font = new System.Drawing.Font("Arial", 10F);
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(741, 161);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowCheckBox = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(265, 27);
            this.dateTimePicker1.TabIndex = 14;
            // 
            // LastFetchDateTime
            // 
            this.LastFetchDateTime.AutoSize = true;
            this.LastFetchDateTime.Font = new System.Drawing.Font("Arial", 10F);
            this.LastFetchDateTime.Location = new System.Drawing.Point(585, 167);
            this.LastFetchDateTime.Name = "LastFetchDateTime";
            this.LastFetchDateTime.Size = new System.Drawing.Size(125, 19);
            this.LastFetchDateTime.TabIndex = 29;
            this.LastFetchDateTime.Text = "Log Fetch From";
            // 
            // DataBackup
            // 
            this.DataBackup.Location = new System.Drawing.Point(810, 717);
            this.DataBackup.Name = "DataBackup";
            this.DataBackup.Size = new System.Drawing.Size(166, 44);
            this.DataBackup.TabIndex = 20;
            this.DataBackup.Text = "DATA BACKUP";
            this.DataBackup.UseVisualStyleBackColor = true;
            this.DataBackup.Click += new System.EventHandler(this.button1_Click);
            // 
            // ImportDatabase
            // 
            this.ImportDatabase.Location = new System.Drawing.Point(1001, 717);
            this.ImportDatabase.Name = "ImportDatabase";
            this.ImportDatabase.Size = new System.Drawing.Size(166, 44);
            this.ImportDatabase.TabIndex = 21;
            this.ImportDatabase.Text = "IMPORT DATABASE";
            this.ImportDatabase.UseVisualStyleBackColor = true;
            this.ImportDatabase.Click += new System.EventHandler(this.ImportDatabase_Click);
            // 
            // SEARCH
            // 
            this.SEARCH.Location = new System.Drawing.Point(741, 220);
            this.SEARCH.Name = "SEARCH";
            this.SEARCH.Size = new System.Drawing.Size(166, 44);
            this.SEARCH.TabIndex = 22;
            this.SEARCH.Text = "SEARCH";
            this.SEARCH.UseVisualStyleBackColor = true;
            this.SEARCH.Click += new System.EventHandler(this.SEARCH_Click);
            // 
            // CompanyName
            // 
            this.CompanyName.AutoSize = true;
            this.CompanyName.Font = new System.Drawing.Font("Arial", 10F);
            this.CompanyName.Location = new System.Drawing.Point(111, 24);
            this.CompanyName.Name = "CompanyName";
            this.CompanyName.Size = new System.Drawing.Size(126, 19);
            this.CompanyName.TabIndex = 23;
            this.CompanyName.Text = "Company Name";
            // 
            // Company_Name
            // 
            this.Company_Name.Font = new System.Drawing.Font("Arial", 12F);
            this.Company_Name.Location = new System.Drawing.Point(256, 24);
            this.Company_Name.Name = "Company_Name";
            this.Company_Name.Size = new System.Drawing.Size(247, 30);
            this.Company_Name.TabIndex = 1;
            // 
            // EmailAddress
            // 
            this.EmailAddress.AutoSize = true;
            this.EmailAddress.Font = new System.Drawing.Font("Arial", 10F);
            this.EmailAddress.Location = new System.Drawing.Point(585, 29);
            this.EmailAddress.Name = "EmailAddress";
            this.EmailAddress.Size = new System.Drawing.Size(113, 19);
            this.EmailAddress.TabIndex = 24;
            this.EmailAddress.Text = "Email Address";
            // 
            // Email_Address
            // 
            this.Email_Address.Font = new System.Drawing.Font("Arial", 12F);
            this.Email_Address.Location = new System.Drawing.Point(741, 24);
            this.Email_Address.Name = "Email_Address";
            this.Email_Address.Size = new System.Drawing.Size(265, 30);
            this.Email_Address.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10F);
            this.label2.Location = new System.Drawing.Point(1101, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 19);
            this.label2.TabIndex = 25;
            this.label2.Text = "Phone Number";
            // 
            // Phone_Number
            // 
            this.Phone_Number.Font = new System.Drawing.Font("Arial", 12F);
            this.Phone_Number.Location = new System.Drawing.Point(1269, 22);
            this.Phone_Number.Name = "Phone_Number";
            this.Phone_Number.Size = new System.Drawing.Size(258, 30);
            this.Phone_Number.TabIndex = 3;
            // 
            // ServiceRunTime
            // 
            this.ServiceRunTime.AutoSize = true;
            this.ServiceRunTime.Font = new System.Drawing.Font("Arial", 10F);
            this.ServiceRunTime.Location = new System.Drawing.Point(1114, 167);
            this.ServiceRunTime.Name = "ServiceRunTime";
            this.ServiceRunTime.Size = new System.Drawing.Size(137, 19);
            this.ServiceRunTime.TabIndex = 30;
            this.ServiceRunTime.Text = "Service Run Time";
            // 
            // Minute
            // 
            this.Minute.AutoSize = true;
            this.Minute.Font = new System.Drawing.Font("Arial", 10F);
            this.Minute.Location = new System.Drawing.Point(1403, 167);
            this.Minute.Name = "Minute";
            this.Minute.Size = new System.Drawing.Size(57, 19);
            this.Minute.TabIndex = 32;
            this.Minute.Text = "Minute";
            // 
            // ScheduleTime
            // 
            this.ScheduleTime.Font = new System.Drawing.Font("Arial", 12F);
            this.ScheduleTime.Location = new System.Drawing.Point(1269, 160);
            this.ScheduleTime.Name = "ScheduleTime";
            this.ScheduleTime.Size = new System.Drawing.Size(120, 30);
            this.ScheduleTime.TabIndex = 33;
            // 
            // notification
            // 
            this.notification.Location = new System.Drawing.Point(12, 12);
            this.notification.Name = "notification";
            this.notification.Size = new System.Drawing.Size(53, 23);
            this.notification.TabIndex = 34;
            this.notification.Text = "value";
            this.notification.UseVisualStyleBackColor = true;
            this.notification.Click += new System.EventHandler(this.notification_Click);
            // 
            // ActiveNotification
            // 
            this.ActiveNotification.Location = new System.Drawing.Point(12, 53);
            this.ActiveNotification.Name = "ActiveNotification";
            this.ActiveNotification.Size = new System.Drawing.Size(53, 23);
            this.ActiveNotification.TabIndex = 35;
            this.ActiveNotification.Text = "value";
            this.ActiveNotification.UseVisualStyleBackColor = true;
            this.ActiveNotification.Click += new System.EventHandler(this.ActiveNotification_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1854, 791);
            this.Controls.Add(this.ActiveNotification);
            this.Controls.Add(this.notification);
            this.Controls.Add(this.ScheduleTime);
            this.Controls.Add(this.Minute);
            this.Controls.Add(this.ServiceRunTime);
            this.Controls.Add(this.Phone_Number);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Email_Address);
            this.Controls.Add(this.EmailAddress);
            this.Controls.Add(this.Company_Name);
            this.Controls.Add(this.CompanyName);
            this.Controls.Add(this.SEARCH);
            this.Controls.Add(this.ImportDatabase);
            this.Controls.Add(this.DataBackup);
            this.Controls.Add(this.LastFetchDateTime);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.IsAttendance);
            this.Controls.Add(this.IsAccessControl);
            this.Controls.Add(this.IsActive);
            this.Controls.Add(this.txt_SerailNumber);
            this.Controls.Add(this.IsManual);
            this.Controls.Add(this.PING);
            this.Controls.Add(this.AccessCtrlUUID);
            this.Controls.Add(this.device_name);
            this.Controls.Add(this.DeviceName);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.btn_update);
            this.Controls.Add(this.port);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.device_uuid);
            this.Controls.Add(this.IPAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_add);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IPAddress;
        private System.Windows.Forms.TextBox device_uuid;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Label DeviceName;
        private System.Windows.Forms.TextBox device_name;
        private System.Windows.Forms.TextBox AccessCtrlUUID;
        private System.Windows.Forms.Button PING;
        private System.Windows.Forms.CheckBox IsManual;
        private System.Windows.Forms.TextBox txt_SerailNumber;
        private System.Windows.Forms.CheckBox IsActive;
        private System.Windows.Forms.CheckBox IsAccessControl;
        private System.Windows.Forms.CheckBox IsAttendance;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label LastFetchDateTime;
        private System.Windows.Forms.Button DataBackup;
        private System.Windows.Forms.Button ImportDatabase;
        private System.Windows.Forms.Button SEARCH;
        private System.Windows.Forms.Label CompanyName;
        private System.Windows.Forms.TextBox Company_Name;
        private System.Windows.Forms.Label EmailAddress;
        private System.Windows.Forms.TextBox Email_Address;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Phone_Number;
        private System.Windows.Forms.Label ServiceRunTime;
        private System.Windows.Forms.Label Minute;
        private System.Windows.Forms.NumericUpDown ScheduleTime;
        private System.Windows.Forms.Button notification;
        private System.Windows.Forms.Button ActiveNotification;
    }
}

