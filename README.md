# SoftwareDevelopmentFundamentalsTest

## About
Simple Web Application Created to test candidates for the first 3 units of the Software Development Fundamentals(C#) MTA. The application is running off of an XML document as no access to SQL.

Questions are randomized for fairness.

## Setup
_requires Visual Studio and ASP.NET Version 4.5_

1. Clone repo 
2. Open Solution (.sln file) in Visual Studio
3. Press run or F5
4. Open your browser to [http://localhost:61176/](http://localhost:61176/)

## Modyfying Questions
Within, either, Visual Studio or a text editor of your choice:

1. Open the **Questions.xml** file located under the **SoftwareDevelopmentTest** folder
2. Modify a Unit using following the structure (Please note, there is no functionality currently available supporting custom units):
  ```XML
  <Unit>
    <Question Id="{UNIQUE INTEGER FOR SCOPE OF UNIT}">
      <Text>
        {TEXT TO BE SHOWN}
      </Text>
      <Answers>
        <{LETTER OR NUMBER REPRESENTATION}>
          {ANSWER TEXT TO BE DISPLAYED}
        </{LETTER OR NUMBER REPRESENTATION}>
        <!-- Repeat Above Syntax as many times as desired -->
      </Answers>
      <Correct>
        <Option>{CORRECT LETTER OR NUMBER REPRESENTATION}</Option>
        <Explanation>
          {TEXT DESCRIBING WHY SPECIFIED OPTION IS CORRECT}<!-- Optional -->
        </Explanation>
      </Correct>
    </Question>
  </Unit>
  ```
3. Save the edited XML file
4. Rebuild Solution and view in browser ([http://localhost:61176/](http://localhost:61176/))

**Source code available to allow for use and or modifications.**

_Project was made to further my knowledge before the final test given on my MTA module for my Level 3 Apprenticeship_
