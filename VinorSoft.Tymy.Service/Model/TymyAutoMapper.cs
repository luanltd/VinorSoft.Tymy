using AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Text;
using VinorSoft.Tymy.Service.Entities;

namespace VinorSoft.Tymy.Service.Model
{
    public class TymyAutoMapper : Profile
    {
        public TymyAutoMapper()
        {
            CreateMap<Orders, OrderModel>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailModel>().ReverseMap();

        }
    }
}
