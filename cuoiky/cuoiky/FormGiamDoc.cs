using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cuoiky
{
    public partial class FormGiamDoc : Form
    {
        
        private int maNV; // lưu mã nhân viên đăng nhập
        string constr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
        public FormGiamDoc(int maNV = 0) // constructor có tham số
        {
            InitializeComponent();
            this.maNV = maNV;
        }
        public FormGiamDoc()
        {
            InitializeComponent();
        }
        private void LoadDanhSachNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = @"SELECT MaNV, HoTen, Phai, NgaySinh, SoDienThoai, 
                               MaSoThue, MaPhongBan, MaVaiTro, LuongCoBan, PhuCap 
                               FROM NhanVien";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvNhanVien.DataSource = dt;
            }
            // Cho phép giám đốc thấy và sửa lương
            dgvNhanVien.ReadOnly = false;
            foreach (DataGridViewColumn col in dgvNhanVien.Columns)
            {
                if (col.Name == "LuongCoBan" || col.Name == "PhuCap")
                    col.ReadOnly = false;
                else
                    col.ReadOnly = true;
            }
        }
        private void FormGiamDoc_Load(object sender, EventArgs e)
        {
            this.Text = "Giao diện Giám đốc - Mã NV: " + maNV;
            LoadDanhSachNhanVien();
            LoadPhongBan();
            LoadComboBoxVaiTro();
            LoadAnhNhanVien(this.maNV);
            LoadLichSu();
        }
        private void LoadAnhNhanVien(int maNV)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT HinhAnh FROM NhanVien WHERE MaNV = @manv";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@manv", maNV);

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string fileName = result.ToString().Trim();

                        // 1. SỬA TẠI ĐÂY: Dùng Path.Combine để tự động xử lý dấu gạch chéo chuẩn xác
                        string path = Path.Combine(Application.StartupPath, "Images", fileName);
                        if (File.Exists(path))
                        {
                            // Giải phóng ảnh cũ để tránh rác bộ nhớ
                            if (pictureBox2.Image != null) pictureBox2.Image.Dispose();

                            // 2. SỬA TẠI ĐÂY: Dùng FileStream thay vì Image.FromFile
                            // Cách này giúp file ảnh không bị "khóa", bạn có thể sửa/xóa file ảnh lúc app đang chạy
                            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                            {
                                pictureBox2.Image = Image.FromStream(fs);
                            }

                            // 3. THAY ĐỔI: Dùng Zoom thay vì StretchImage để ảnh không bị méo mặt
                            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        else
                        {
                            // Thông báo để bạn kiểm tra folder bin\Debug\Images
                            MessageBox.Show("Không tìm thấy ảnh tại: " + path);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi load ảnh: " + ex.Message);
                }
            }
        }
        private void LoadComboBoxVaiTro()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string sql = "SELECT * FROM VaiTro";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboVaiTro.DataSource = dt;
                cboVaiTro.DisplayMember = "TenVaiTro";
                cboVaiTro.ValueMember = "MaVaiTro";
            }
        }
        private void LoadPhongBan()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = "SELECT MaPhongBan, TenPhongBan FROM PhongBan";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboPhongBan.DataSource = dt;
                cboPhongBan.DisplayMember = "TenPhongBan";
                cboPhongBan.ValueMember = "MaPhongBan";
            }
        }
  

        private void btnThem_Click(object sender, EventArgs e)
        {

        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        // Khi người dùng click 1 dòng
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                // 1. Đổ dữ liệu lên các ô (Giám đốc xem được tất cả)
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                txtPhai.Text = row.Cells["Phai"].Value.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtMaSoThue.Text = row.Cells["MaSoThue"].Value.ToString();
                cboPhongBan.SelectedValue = row.Cells["MaPhongBan"].Value;
                cboVaiTro.SelectedValue = row.Cells["MaVaiTro"].Value;

                // 2. Hiện Lương và Phụ cấp (Sử dụng tên cột LuongCoBan bạn đã fix lúc nãy)
                txtLuong.Text = row.Cells["LuongCoBan"].Value.ToString();
                txtPhuCap.Text = row.Cells["PhuCap"].Value.ToString();
                // 3. Khóa các ô thông tin, chỉ mở ô Lương/Phụ cấp
                txtHoTen.ReadOnly = true;
                txtPhai.ReadOnly = true;
                txtSDT.ReadOnly = true;
                txtMaSoThue.ReadOnly = true;
                // Khóa không cho người dùng tương tác với ComboBox
                cboVaiTro.Enabled = false;
                cboPhongBan.Enabled = false;
                // 2. KHÓA ô ngày sinh (Không cho Giám đốc chỉnh sửa)
                dtpNgaySinh.Enabled = false;
                txtLuong.ReadOnly = false; // Giám đốc được quyền sửa
                txtPhuCap.ReadOnly = false; // Giám đốc được quyền sửa
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để cập nhật!");
                return;
            }
            int ma = Convert.ToInt32(dgvNhanVien.SelectedRows[0].Cells["MaNV"].Value);
            decimal luong = 0;
            decimal phuCap = 0;

            // Kiểm tra txtLuong: Nếu không phải là số hợp lệ thì dừng lại và báo lỗi
            if (!decimal.TryParse(txtLuong.Text.Trim(), out luong))
            {
                MessageBox.Show("Vui lòng nhập số lương hợp lệ (không chứa chữ hoặc ký tự lạ)!");
                txtLuong.Focus();
                return;
            }

            // Kiểm tra txtPhuCap: Tương tự như trên
            if (!decimal.TryParse(txtPhuCap.Text.Trim(), out phuCap))
            {
                MessageBox.Show("Vui lòng nhập số phụ cấp hợp lệ!");
                txtPhuCap.Focus();
                return;
            }
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = @"UPDATE NhanVien SET LuongCoBan=@luong, PhuCap=@pc WHERE MaNV=@ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@luong", luong);
                cmd.Parameters.AddWithValue("@pc", phuCap);
                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.ExecuteNonQuery();
            }
            GhiLichSu(ma, "Giám đốc", "Cập nhật lương", $"Thay đổi lương {luong}, phụ cấp {phuCap}");
            LoadDanhSachNhanVien();
            MessageBox.Show("Đã cập nhật lương và phụ cấp thành công!");
        }
        private void GhiLichSu(int maNV, string nguoi, string hanhDong, string chiTiet)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = "INSERT INTO LichSuHoatDong (MaNV, TenDangNhap, HanhDong, ChiTiet) VALUES (@manv, @user, @hd, @ct)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@manv", maNV);
                cmd.Parameters.AddWithValue("@user", nguoi);
                cmd.Parameters.AddWithValue("@hd", hanhDong);
                cmd.Parameters.AddWithValue("@ct", chiTiet);
                cmd.ExecuteNonQuery();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            FormLogin login = new FormLogin();
            login.Show();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dgvLichSu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadLichSu(string keyword = "")
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = @"SELECT L.ID, L.MaNV, N.HoTen AS NhanVienLienQuan, 
                        L.TenDangNhap, L.HanhDong, L.ChiTiet, L.ThoiGian
                        FROM LichSuHoatDong L
                        LEFT JOIN NhanVien N ON L.MaNV = N.MaNV
                        WHERE L.TenDangNhap LIKE @key 
                        OR L.HanhDong LIKE @key 
                        OR L.ChiTiet LIKE @key
                        ORDER BY L.ThoiGian DESC";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                // Truyền giá trị tìm kiếm vào, dùng dấu % để tìm kiếm gần đúng
                da.SelectCommand.Parameters.AddWithValue("@key", "%" + keyword + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvLichSu.DataSource = dt;
                // Tự động giãn các cột vừa khít với nội dung
                dgvLichSu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Ưu tiên cột "Chi tiết" rộng nhất để đọc được hết thao tác
                dgvLichSu.Columns["ChiTiet"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                // Nếu nội dung quá dài, cho phép xuống dòng trong ô
                dgvLichSu.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvLichSu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
           
            dgvLichSu.Columns["ID"].HeaderText = "Mã HS";
            dgvLichSu.Columns["NhanVienLienQuan"].HeaderText = "NV liên quan";
            dgvLichSu.Columns["TenDangNhap"].HeaderText = "Người thực hiện";
            dgvLichSu.Columns["HanhDong"].HeaderText = "Hành động";
            dgvLichSu.Columns["ChiTiet"].HeaderText = "Chi tiết";
            dgvLichSu.Columns["ThoiGian"].HeaderText = "Thời gian";
        }

      

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click_1(object sender, EventArgs e)
        {

        }

        private void txtPhai_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void txtTimKiemLichSu_TextChanged(object sender, EventArgs e)
        {
            // Lấy nội dung từ TextBox tìm kiếm truyền vào hàm Load
            LoadLichSu(txtTimKiemLichSu.Text.Trim());
        }
        private void ClearFields()
        {
            txtHoTen.Clear();
            txtPhai.Clear();
            txtSDT.Clear();
            txtMaSoThue.Clear();
            dtpNgaySinh.Value = DateTime.Now; // Trả về ngày hiện tại

            // Đặt lại các ComboBox về mục đầu tiên (nếu có dữ liệu)
            if (cboPhongBan.Items.Count > 0) cboPhongBan.SelectedIndex = 0;
            if (cboVaiTro.Items.Count > 0) cboVaiTro.SelectedIndex = 0;

            // Trạng thái mặc định cho Lương và Phụ cấp theo yêu cầu của bạn
            txtLuong.Text = "null";
            txtPhuCap.Text = "null";

            // Đưa con trỏ chuột về ô Họ tên để nhập mới cho nhanh
            txtHoTen.Focus();
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}
