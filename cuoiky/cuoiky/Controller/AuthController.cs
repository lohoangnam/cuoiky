using System;
using cuoiky.Repositories;

namespace cuoiky.Controllers
{
    public class AuthController
    {
        private AuthRepository _authRepo;

        public AuthController()
        {
            _authRepo = new AuthRepository();
        }

        // Hàm Login thực hiện Validation và gọi sang Repository
        public bool Login(string username, string password, out int maNV, out string tenVaiTro, out string tenPhongBan)
        {
            // Nếu Validation (kiểm tra rỗng) có lỗi, dừng ngay lập tức
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                maNV = 0;
                tenVaiTro = "";
                tenPhongBan = "";
                return false;
            }

            // Gọi hàm từ Repository
            return _authRepo.KiemTraDangNhap(username, password, out maNV, out tenVaiTro, out tenPhongBan);
        }
    }
}