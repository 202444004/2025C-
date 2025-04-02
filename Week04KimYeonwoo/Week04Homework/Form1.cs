using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Week04Homework
{
    public partial class FormManager: Form
    {
        Department[] departments;   //instance field (변수), 멤버 변수
        List<Professor> professors;

        //생성자
        //인슽턴스 생성시 초기화가 필요한 코드 넣음.
        public FormManager()
        {
            InitializeComponent();
            departments = new Department[10];   //오른쪽이 배열임
            professors = new List<Professor>();
        } //화면 세팅 끝

        
        private void btnRegiserDepartment_Click(object sender, EventArgs e)
        {
            int index = -1; //index = 0 -> 0이 채워지는지 모름
            for(int i = 0; i < departments.Length; i++)
            {
                if (departments[i] == null)
                {
                    if (index < 0) { 
                        index = i;
                    }
                    //break; //break 없으면 뒤부터 있으면 앞부터 채움.
                }
                else
                {
                    if (departments[i].Code == tbxDepartmentCode.Text) //중복 입력시
                    {
                        //메세지 띄우기 과제
                        return;
                    }
                }
            }

            if (index < 0)
            {
                //메세지 띄우기 과제
                return;
            }

            Department dept = new Department();
            dept.Code = tbxDepartmentCode.Text;
            dept.Name = tbxDepartmentName.Text;

            departments[index] = dept;

            //추후 아래 3문장 지운다.
            lbxDepartment.Items.Add(dept);
            lbxDepartment.Items.Add(dept.Code);
            lbxDepartment.Items.Add(dept.Name);
            lbxDepartment.Items.Add($"[{dept.Code}] {dept.Name}");
        }

        private void btnRemoveDepartment_Click(object sender, EventArgs e)
        {
            if (lbxDepartment.SelectedIndex < 0) //클릭한 것만 삭제해야함
            {
                //메세지 띄우기 과제
                return;
            }

            //is 연산자, 타입 확인용으로 사용
            if (lbxDepartment.SelectedItem is Department)
            {
                var target = (Department)lbxDepartment.SelectedItem; //target type -> department


                for(int i = 0; i < departments.Length; i++)
                {
                    if (departments[i] != null && departments[i] == target)
                    {
                        departments[i] = null;
                        break;
                    }
                }

                lbxDepartment.Items.RemoveAt(lbxDepartment.SelectedIndex);
                lbxDepartment.SelectedIndex = -1; //아무것도 선택하지 않은 상태로 만들어줌
            }
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabMain.SelectedIndex)
            {
                case 0:
                    break;
                case 1: //교수정보
                    cmbProfessorDepartment.Items.Clear(); //최신 데이터 유지
                    foreach (var department in departments)
                    {
                        if (department != null) { 
                            cmbProfessorDepartment.Items.Add(department);
                        }
                    }

                    cmbProfessorDepartment.SelectedIndex = -1;
                    lbxProfessor.Items.Clear();

                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }

        private void cmbProfessorDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            //index값 검사해서 진행여부 결정

            lbxProfessor.Items.Clear();

            //as 형변환 연산자
            //형변환이 정상적이지 않으면 null반환
            var department = cmbProfessorDepartment.SelectedItem as Department;

            if (department != null)
            {
                foreach(var professor in professors)
                {
                    //& -> 둘다 검사 && -> 앞이 틀리면 뒤는 검사 안함 || -> 
                    if (professor != null && professor.DepartmentCode == department.Code) //professor != null 검사
                    {
                        lbxProfessor.Items.Add(professor);
                    }
                }
            }
        }
    }
}
