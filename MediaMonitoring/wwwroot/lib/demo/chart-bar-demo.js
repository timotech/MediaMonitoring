// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

// Bar Chart Example
var ctx = document.getElementById("myBarChart");
var myLineChart = new Chart(ctx, {
  type: 'bar',
  data: {
    labels: ["M.T.N", "Airtel NG", "Nestle NG", "Globacom NG", "Reckitt Benckizer", "9Mobile","Multichoice Nigeria","Sundry Ad"],
    datasets: [{
      label: "Revenue",
      backgroundColor: "#00C3FF",
      borderColor: "rgba(2,117,216,1)",
      data: [6215, 4312, 3251, 7841, 6821, 5984,4984,3984],
    }],
  },
  options: {
    scales: {
      xAxes: [{
        time: {
          unit: 'month'
        },
        gridLines: {
          display: false
        },
        ticks: {
          maxTicksLimit: 8
        }
      }],
      yAxes: [{
        ticks: {
          min: 0,
          max: 10000,
          maxTicksLimit: 8
        },
        gridLines: {
          display: true
        }
      }],
    },
    legend: {
      display: false
    }
  }
});
