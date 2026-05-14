using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace cuoiky
{
    public partial class FormLogin : Form
    {
        string constr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                // SỬA TẠI ĐÂY: JOIN qua bảng NhanVien để lấy MaVaiTro mới nhất
                string sql = @"SELECT nv.MaNV, vt.TenVaiTro 
                       FROM NguoiDungDangNhap nd
                       JOIN NhanVien nv ON nd.MaNV = nv.MaNV
                       JOIN VaiTro vt ON nv.MaVaiTro = vt.MaVaiTro
                       WHERE nd.TenDangNhap = @user AND nd.MatKhau = @pass";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@pass", pass);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string role = dr["TenVaiTro"].ToString();
                    int maNV = Convert.ToInt32(dr["MaNV"]);

                    this.Hide(); // Ẩn form đăng nhập
                    Form nextForm = null;

                    // Kiểm tra vai trò để khởi tạo Form tương ứng
                    switch (role)
                    {
                        case "Giám đốc":
                            nextForm = new FormGiamDoc(maNV);
                            break;
                        case "Trưởng phòng nhân sự":
                            nextForm = new FormTruongPhongNhanSu(maNV);
                            break;
                        case "Nhân viên phòng nhân sự":
                            nextForm = new FormNhanSu(maNV);
                            break;
                        case "Nhân viên phòng tài vụ":
                            nextForm = new FormTaiVu(maNV);
                            break;
                        case "Trưởng phòng":
                            nextForm = new FormTruongPhong(maNV);
                            break;
                        default:
                            nextForm = new FormNhanVien(maNV);
                            break;
                    }

                    if (nextForm != null)
                    {
                        // Khi Form chức năng đóng lại, hiện lại Form đăng nhập
                        nextForm.ShowDialog();
                        this.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
                }
            }
        }
    }
    
}
