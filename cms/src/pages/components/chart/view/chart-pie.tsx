import Chart from "@/components/chart/chart";
import useChart from "@/components/chart/useChart";

export default function ChartPie({ data }: any) {
  const series = data.map((i: any) => {
    return i.Total;
  });
  const chartOptions = useChart({
    labels: data.map((i: any) => {
      return i.RoleName;
    }),
    legend: {
      horizontalAlign: "center",
    },
    stroke: {
      show: false,
    },
    dataLabels: {
      enabled: true,
      dropShadow: {
        enabled: false,
      },
    },
    tooltip: {
      fillSeriesColor: false,
    },
    plotOptions: {
      pie: {
        donut: {
          labels: {
            show: false,
          },
        },
      },
    },
  });

  return (
    <Chart type="pie" series={series} options={chartOptions} height={320} />
  );
}
