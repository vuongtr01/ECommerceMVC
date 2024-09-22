using System;
using AutoMapper;
using ECommerceMVC.ViewModels;
using ECommerceMVC.Data;

namespace ECommerceMVC.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<RegisterVM, KhachHang>().ReverseMap();
		}
	}
}

