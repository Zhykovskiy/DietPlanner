import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MealService {

  readonly baseUrl = "https://localhost:7212/api";

  constructor(private _http:HttpClient) { }

  generateMealPlan(calories: number) {
    return this._http.get(this.baseUrl + '/Meal/GenerateMealPlan?targetCalories=' + calories);
  }

  addMealToPlan(body: any[]) {
    return this._http.post(this.baseUrl + '/Meal/AddMealToPlan', body);
  }
}
