# DiabetesTracker

# Project Description

Diabetes is a diseas that widespread in today's world. Because of there is no permanent cure for it, people with diabetes should learn how to live their lives and struggle with the difficulties they will face on their own. In other words, they are all alone on it. Our main purposes on that project are give informations to patients about what they need to do according to their test results and Body Mass Index (BMI), create an opportunity of taking ideas from experts on subject. Users must add their personal details such as their ages and weights, medicines they use or details on previous illnesses if they had etc. While registering. Storing this kind of information will help for improve any further website activities. The system also have doctors and fitness instructors which are experts on their areas as user beside diabetic patients. It is expected that users can be able to ask questions to experts on topics that they are curious about. The system will provide exercise plans, eating schedules and also charts about their conditions that changing daily, weekly and monthly based on patients' blood glucose level which updated daily by them. It is demanded from system that create a base which users can contact with others who at same level of disease and mutually help each other basis on their own experiences.

# Specific Requirements
## Functional requirement 1.1
### Title: Sign Up
* 1.1. System shall provide register page for users.
* 1.2. System must provide three login options to obtain account type.
* 1.3. System must redirect user to another page according to chosen account type.
* 1.4. System shall show a message that says users should upload document which can confirm account type to all users.
* 1.5. System shall send user's information to system administration automatically. 
* 1.6. System will provide text boxes according to chosen account type.
* 1.7. System must save name, surname, password, e-mail, phone number, birth date, gender, height, weight, profile picture and short CV.
* 1.8. System shall contain document upload button.
* 1.9. The confirmation document can be Dietician report for diabetes or expert report from an institution.
* 1.10. System must warn users if there is any empty box.

## 2 Functional requirement 1.2
### Title: Sign In
* 2.1. System will show two text boxes.
* 2.2. System must check validity of given information and automatically send to home page.
* 2.3. System must show warning if given information is wrong.

## 3 Functional requirement 1.3
### Title:Account Recovery
* 3.1. System will show a text box.
* 3.2. System must check validity of e-mail address.
* 3.3. System shall send mail that contain recovery link to given e-mail address.
* 3.4. System must show warning if given e-mail address is wrong.
* 3.5. System must redirect users to password changing page that contains two text boxes. 3.6. System will accept password change if two written password equals.

## 4 Functional requirement 1.4
### Title: Adding New Contacts
* 4.1 System must provide a button for adding a user when user see an overview of profile.
* 4.2. If user click that button system must send a friend request to that user. Show notification on requested profile.
* 4.2.1. If other user accepts the friend request users automatically added their contact lists.
* 4.2.2 If other user rejects the friend request, friend request must be deleted.

## 5 Functional requirement 1.5 
### Title: Send Message
* 5.1 System should provide a list of text boxes to obtain users in the system.
* 5.2 System must provide a text box and a send button. Each user should be able to send instant message to any contact on users’ contact list.
* 5.3 System will provide a message box that will display an error when trying to send a message that has more than 400 characters.
* 5.4 System will provide a text box to display the message when the send message button is pressed.

## 6 Functional requirement 1.6
### Title: Show Message
* 6.1 The system should provide a list of incoming messages alongside the message box.
* 6.2 The system should provide a text box showing the user name of the sender of the message and a short preview of the incoming message to the list showing the new incoming messages.
* 6.3 When the system clicks on the incoming message, it will redirect to the message sending page and provide a list of text boxes to display incoming and outgoing messages.
* 6.4 The system should provide text boxes with the date and time that they are sent alongside the text boxes where the messages are located.
 
## 7 Functional requirement 1.7 
### Title: Show Notification
* 7.1. The system must alert user if they are online by pop-ups when there is notification.
* 7.2. The system should provide notification box on the menu where users can view their recent notifications up to date.
* 7.3. The system must send notifications when users' daily and weekly posted. 7.4. The system should allow users to clear their notification list.

## 8 Functional requirement 1.8
### Title: Search Dieticians and Fitness Coaches
* 8.1. The system must let user to search Dieticians and fitness coaches in search bar on the menu.
* 8.2. The system should display profile pictures, names and surnames and specific profession of the entered search.
* 8.3. The system must display error message if there is no matching result.

## 9 Functional requirement 1.9
### Title: Show Users' Best Matches
* 9.1. The system must create a database by given information’s while registering and match the users at same level of disease.
* 9.2. The system should display profile pictures, names and surnames and brief factors that cause the matching of users in the best matches menu.
* 9.3. The system must display accounts by relevance order.
* 9.4. The system should allow users to directly contact by putting send message button in that menu.
* 9.5. The system must update best matches list weekly according to users' data’s.
* 9.6 The only users who has registered as patient must see the best matches. 9.7 Users must see bests matches on their home page.

## 10 Functional requirement 1.10
### Title:Add Diet and Exercise List
* 10.1 System must include a button for adding a Diet and Exercise list. 10.2 System must include text box areas and a save button.
* 10.3 System must allow to add and save Diet and Exercise list for the users who registered as Dieticians or fitness coaches.
* 10.4 Users shall be able to add additional information’s on Diet and Exercise list. These are type of the diabetes, age range, gender, weight range and height range, using insulin or not.
* 10.5 System must save the given additional information and the writer information as a metadata for the Diet and Exercise list.

## 11 Functional requirement 1.11
### Title: Match Diet and Exercise List with Users
* 11.1. System shall be able to compare users’ some information.
* 11.1.1 If the users’ insulin usage and type of the diabetes information’s are unknown system must warn the user for a once and until user enter his/her these information’s system does not match for a Diet and Exercise list.
* 11.1.2 System must compare gender, age, body mass index, type of diabetes and insulin usage information between user and Diet and Exercise lists and save it ones a day if the user online at that day.

## 12 Functional requirement 1.12
### Title: Show the Diet and Exercise List
* 12.1 System must include a text area.
* 12.1.1 System must show the Diet and Exercise list if the user saved a Diet and Exercise list before.
* 12.1.2 System must show a message in that area inform users about this are for the Diet and Exercise list.1
* 12.2 System must include a second text area and a button.
* 12.2.1 System must show the recommended Diet and Exercise lists when user press that button in that area.
* 12.2.2 System must show the writer of the lists below every list. 12.2.3 System must include a save button for every list.
* 12.2.4 System must save the lists for that user.

## 13 Functional requirement 1.13 
### Title: Show Users Diabetes rate
* 13.1 Users will be entering their blood sugar level according to their measurement on blood sugar meter.
* 13.2 The system will be having three levels which are corresponding to low level, normal level, high level.
* 13.3 The system will be Showing a warning message for the user when it is low or high.

# 14 Functional requirement 1.14 
### Title:Entrance for user information
* 14.1. The system will be containing an add previous diseases section in each users personal pages.
* 14.1.1 Users will enter their chronical diseases.
* 14.1.2 Users will enter their surgery history if there exists any.
* 14.2. The system must provide an area for user to add blood sugar level on home page.
* 14.3 The system must provide update button for any blood sugar level, previous diseases and personal information.

## 15 Functional requirement 1.15
### Title:Show popular users who are fitness coaches or Dieticians
* 15.1 System must show popular users who has registered as patient.
* 15.2 There will be a rating star button for each fitness coach. Users will rate them.
* 15.2.1 There will be a Top list on the right of the page who has the most star.

## 16 Functional requirement 1.16 
### Title: Categorize
* 16.1 The system will be having a two different type of diabetes level for users.
* 16.1.1Type 1 will be containing the users who are just diagnosed diabetes patients
* 16.1.2 Type 2 will be containing the users who are using Insulin
* 16.2 The system categorizes users according to their body mass index and their age ranges.

## 17 Functional requirement 1.17
Title: Show Graphical data
* 17.1 The System will be showing the last 7 days entrance for blood sugar.
* 17.2 The system will be calculating the difference for each day by subtracting current blood sugar value from normal blood sugar value the dividing the result with current blood sugar.
* 17.3 The system will be having information about sugar level whether it is on the range or not. If it is not in the range, it will show how far from the normal value.
* 17.3.1 For each day, If the blood sugar is not in the range, it will be red on the graphic.
* 17.3.2 For each day, If the blood sugar is in the range, it will be green on the graphic.
* 17.4 The system will be storing these graphics at most one year in its database. There will be a button to show previous graphics.

## 18 Functional requirement 1.18
Title: See Detailed Profile of Contacts
* 18.1 The system must provide a that user can see their friends detailed information.
* 18.2 The user can see all information except users’ private information such as e-mail, phone number, password, confirmation document and messages.
