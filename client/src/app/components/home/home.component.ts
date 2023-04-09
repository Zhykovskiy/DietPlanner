import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { MealService } from 'src/app/shared/meal.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  calories: number = 0;
  mealSubject = new Subject<any>();
  mealPlan: any;
  showMealPlan = false;
  date: Date | undefined;

  constructor(private router:Router, private _mealService: MealService) { }

  ngOnInit(): void {
  }

  onLogout(){
    localStorage.removeItem('token');
    this.router.navigate(['user/login']);
  }

  generateMealPlan() {
    this._mealService.generateMealPlan(this.calories).subscribe(
        data => {this.mealSubject.next(data); },
        error => { console.log(error)}
      );

    this.mealSubject.subscribe(
      data => {
        this.mealPlan = data;
        this.showMealPlan = true;
        console.log(this.mealPlan);
      },
      error => {
        console.log(error);
      }
    )
  }

  addMealToPlan() {
    var body: any[] = []

    this.mealPlan.meals.forEach((meal: any, i: number) => {
      body[i] = {
        date: this.date,
        slot: i+1,
        position: 0,
        type: "RECIPE",
        value: {
          id: meal.id,
          servings: 1,
          title: meal.title,
          imageType: "jpg"
        }
      }
    });

    this._mealService.addMealToPlan(body);
    console.log(body);
  }

}
