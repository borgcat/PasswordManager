Feature: PasswordManager-AddPassword
	In order to avoid remembering passwords
	As a forgetfull person 
	I can remember a single password or secret
	I want to add to a list of my passwords for all my secured websites I am a member of

@AddAPassword
Scenario: Add a Password to my List
	Given Given I have a secret of 'Test1234'
	And I give my password and site details
	Then the result should be count of passwords greater than zero after I add one
