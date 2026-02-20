Feature: Relative dates in table cells

Tests that step argument transformations can be applied to table cells
to convert expressions like "in 5 days" or "7 days ago" into dates relative to today.

@table-cell-transformations
Scenario: Bind relative dates from table cells
	Given the date ranges
		| ID | Start        | End          |
		|  1 | 164 days ago | 162 days ago |
		|  2 | in 10 days   | in 14 days   |
		|  5 | in 72 days   | in 78 days   |
		|  8 | in 50 days   | in 51 days   |
	Then date range 1 should start 164 days ago and end 162 days ago
	And date range 2 should start in 10 days and end in 14 days
	And date range 5 should start in 72 days and end in 78 days
	And date range 8 should start in 50 days and end in 51 days
