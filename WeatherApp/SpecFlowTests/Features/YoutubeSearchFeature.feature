Feature: YoutubeSearchFeature
	In order to test search functionality on youtube
	As a developer
	I want to ensure functionality is working end to end

@scopedBinding
Scenario: Youtube should search for the given keyboard and should navigate to search results page
	Given I have navigated to youtube website
	And I have entered <SearchTerm1> as search keyword
	When I press the search button
	Then I should be redirected to search results page

@source
Examples:
| SearchTerm1 |
| Scotland |