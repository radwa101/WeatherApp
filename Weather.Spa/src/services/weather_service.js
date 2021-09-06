import utilsRepo from "./utils";

class WeatherService {
    getWorldWeather()  {
      	return utilsRepo.get("http://localhost:83/WeatherService.svc/GetWorldTemperatures");
	}

	getEuropeanWeather()  {
		  return utilsRepo.get("http://localhost:83/WeatherService.svc/GetEuropeanTemperatures");
	}

	getIrelandWeather()  {
		  return utilsRepo.get("http://localhost:84/api/IrelandTemperatues");
	}

    getExtremeWeather(filter)  {
		return utilsRepo.get("http://localhost:84/api/extremes?" + filter);
	}

	getForecastData(filter)  {
		return utilsRepo.get("http://localhost:83/WeatherService.svc/GetForecastData?" + filter);
	}

    //put(avatar: string)  {
    //   	return utilsRepo.put({source: avatar} , "/user/settings/avatar/");
	//}
	// public update(settings: any)  {
    //   	return utilsRepo.put({settings} , "/user/settings");
	// }
}
export { WeatherService };