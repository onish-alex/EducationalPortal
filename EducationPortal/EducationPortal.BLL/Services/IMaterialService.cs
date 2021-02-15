using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Response;

namespace EducationPortal.BLL.Services
{
    public interface IMaterialService : IService
    {
        //void GetByName();
        AddMaterialResponse AddMaterial(MaterialDTO material);

        GetMaterialsResponse GetAllMaterials();

        GetMaterialsResponse GetByIds(long[] ids);
    }
}
