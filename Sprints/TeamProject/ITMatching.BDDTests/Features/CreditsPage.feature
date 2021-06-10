Feature: Credits Page

Scenario: As a user, I want to view the credits page of the website
	Given Currently, I am on the home page
	When I click on the credits page link
	Then display the credits page

Scenario: As a user, I want to see next slide on the credits page of the website
	Given Currently, I am on the credits page
	When I click on the next arrow button
	Then change the photo, title and description

Scenario: As a user, I want to see prev slide on the credits page of the website
	Given Currently, I am on the credits page
	When I click on the prev arrow button
	Then change the photo, title and description